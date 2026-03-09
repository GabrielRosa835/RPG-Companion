using RpgCompanion.Application.Engines;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

using Utils.UnionTypes;

namespace RpgCompanion.Application;

internal class Engine
{
   private readonly PluginManager _pluginManager = default!;
   private readonly ContextValidator _validator = default!;
   private readonly RuleCollection _rules = default!;
   private readonly EffectQueue _effects = default!;
   private readonly EventQueue _queue = default!;
   private readonly Reflect _reflect = default!;

   public async Task Execute(PluginDescriptor plugin)
   {
      while(!_queue.Any())
      {
         await Task.Delay(100);
      }

      // Preparation

      if (await _pluginManager.Load(plugin).IsFailure())
      {
         return;
      }

      var @event = _queue.Dequeue();
      var eventType = @event.GetType();
      var context = new Context(plugin);

      var template = plugin.Registry.GetTemplate(eventType);
      var contract = plugin.Registry.GetContract(eventType);

      _rules.SetValues(plugin.Registry.GetRules(eventType));
      _effects.SetValues(plugin.Registry.GetEffects(eventType));

      // Execution

      if (template is not null)
      {
         _reflect.TemplateBundle(template.GenericType)?.Invoke(template.Instance, [context]);
      }

      foreach (var rule in _rules.BeforeEvent())
      {
         var ruleEvent = _reflect
            .RuleApply(rule.GenericType)
            .Invoke(rule, [context])!
            .As<IEvent>();
         _queue.Enqueue(ruleEvent);
      }

      if (contract is not null)
      {
         _validator.ValidateInputs(contract, context);
      }

      while(_effects.Count > 0)
      {
         var effect = _effects.Dequeue();
         _reflect.EffectApply(effect.GenericType)
            .Invoke(effect, [@event, context]);
      }

      if (contract is not null)
      {
         _validator.ValidateOutputs(contract, context);
      }

      foreach (var rule in _rules.AfterEvent())
      {
         var ruleEvent = _reflect
            .RuleApply(rule.GenericType)
            .Invoke(rule, [context])!
            .As<IEvent>();
         _queue.Enqueue(ruleEvent);
      }

      // Cleanup

      _rules.Clear();
      _effects.Clear();
   }
}