using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

using System.Collections;

using Utils.UnionTypes;

namespace RpgCompanion.Application;

internal class Engine
{
   private readonly PluginManager _pluginManager = default!;
   private readonly EventQueue _queue = default!;

   public async Task Execute(PluginDescriptor plugin)
   {
      while(!_queue.Any())
      {
         await Task.Delay(100);
      }

      if (await ActivatePlugin(plugin, _pluginManager).IsFailure())
      {
         return;
      }

      IEvent @event = _queue.Dequeue();
      var context = new Context(plugin);
      var validator = new ContextValidator();

      await BundleContext(@event, context, plugin.Registry);

      var contract = plugin.Registry.GetContract(@event);

      if (contract is not null)
      {
         validator.ValidateInputs(context, contract, @event.GetType());
      }

      await ApplyEffects(@event, context, plugin.Provider);

      if (contract is not null)
      {
         validator.ValidateOutputs(context, contract, @event.GetType());
      }
   }

   private static async Task<Attempt> ActivatePlugin(PluginDescriptor plugin, PluginManager manager)
   {
      if (plugin.Activated)
      {
         return Results.Success();
      }
      return manager.Load(plugin);
   }

   private static async Task BundleContext(IEvent @event, IContext context, ComponentProvider registry)
   {
      var template = registry.GetTemplate(@event);
      var bundle = template?.GetType().GetMethod(nameof(IContextTemplate<>.Bundle));
      bundle?.Invoke(template, [context]);
   }

   private static async Task ApplyEffects(IEvent @event, IContext context, IServiceProvider provider)
   {
      var effectType = typeof(IEffect<>).MakeGenericType(@event.GetType());

      IEnumerable effects = provider.GetServices(effectType);

      var applyEffect = effectType.GetMethod(nameof(IEffect<>.Apply));

      foreach (var effect in effects)
      {
         applyEffect?.Invoke(effect, [@event, context]);
      }
   }
}