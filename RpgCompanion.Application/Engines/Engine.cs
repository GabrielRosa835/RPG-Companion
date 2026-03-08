using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Application.Engines;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

using System.Collections;
using System.Reflection;

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

      if (await _pluginManager.Load(plugin).IsFailure())
      {
         return;
      }

      // TODO: Reflection method cache

      var @event = _queue.Dequeue();
      var eventType = @event.GetType();
      var context = new Context(plugin);
      var validator = new ContextValidator();
      var rules = new RuleCollection(plugin.Registry.GetRules(eventType));
      var effects = new EffectQueue(plugin.Registry.GetEffects(eventType));
      var ruleApply = typeof(IRule<>).MakeGenericType(eventType).GetMethod(nameof(IRule<>.Apply))!;
      var effectApply = typeof(IEffect<>).MakeGenericType(eventType).GetMethod(nameof(IEffect<>.Apply))!;
      var templateBundle = typeof(IContextTemplate<>).MakeGenericType(eventType).GetMethod(nameof(IContextTemplate<>.Bundle))!;

      var template = plugin.Registry.GetTemplate(eventType);
      templateBundle.Invoke(template, [context]);

      foreach (object rule in rules.BeforeEvent())
      {
         var event2 = (IEvent) ruleApply.Invoke(rule, [context])!;
         _queue.Enqueue(event2);
      }

      var contract = plugin.Registry.GetContract(eventType);

      if (contract is not null)
      {
         validator.ValidateInputs(context, contract, eventType);
      }

      while(effects.Count > 0)
      {
         var effect = effects.Dequeue();
         effectApply.Invoke(effect, [context]);
      }

      if (contract is not null)
      {
         validator.ValidateOutputs(context, contract, eventType);
      }

      foreach (object rule in rules.AfterEvent())
      {
         var event2 = (IEvent) ruleApply.Invoke(rule, [context])!;
         _queue.Enqueue(event2);
      }
   }
}