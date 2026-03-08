using RpgCompanion.Application.Engines;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

using Utils.UnionTypes;

namespace RpgCompanion.Application;

internal class Engine
{
   private readonly PluginManager _pluginManager = default!;
   private readonly EventQueue _queue = default!;
   private readonly Reflect _reflect = default!;
   private readonly ContextValidator _validator = default!;


   public async Task Execute(PluginDescriptor plugin)
   {
      while(!_queue.Any())
      {
         await Task.Delay(100);
      }

      if (await _pluginManager.Load(plugin).IsFailure())
      {
         return;
      }

      // TODO: Reflection method cache

      var @event = _queue.Dequeue();
      var eventType = @event.GetType();
      var context = new Context(plugin);
      var rules = new RuleCollection(plugin.Registry.GetRules(eventType));
      var effects = new EffectQueue(plugin.Registry.GetEffects(eventType));

      var template = plugin.Registry.GetTemplate(eventType);
      if (template is not null)
      {
         var method = template.GenericType.GetMethod(nameof(IContextTemplate<>.Bundle));
         method?.Invoke(template.Instance, [context]);
      }

      foreach (var rule in rules.BeforeEvent())
      {
         var event2 = (IEvent) _reflect.RuleApply(rule.GenericType).Invoke(rule, [context])!;
         _queue.Enqueue(event2);
      }

      var contract = plugin.Registry.GetContract(eventType);

      if (contract is not null)
      {
         _validator.ValidateInputs(contract, context);
      }

      while(effects.Count > 0)
      {
         var effect = effects.Dequeue();
         _reflect.EffectApply(effect.GenericType).Invoke(effect, [context]);
      }

      if (contract is not null)
      {
         _validator.ValidateOutputs(contract, context);
      }

      foreach (var rule in rules.AfterEvent())
      {
         var event2 = (IEvent) _reflect.RuleApply(rule.GenericType).Invoke(rule, [context])!;
         _queue.Enqueue(event2);
      }
   }
}