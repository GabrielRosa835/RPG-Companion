using RpgCompanion.Application.Engines;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using Utils.Extensions;
using Utils.UnionTypes;

namespace RpgCompanion.Application;

internal class Engine
{
   private readonly PluginManager _pluginManager = default!;
   private readonly ContextValidator _validator = default!;
   private readonly Reflect _reflect = default!;
   
   private readonly List<ComponentDescriptor> _rules = default!;
   private readonly EventQueue _queue = default!;

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
      var effect   = plugin.Registry.GetEffect(eventType);
      _rules.Set(plugin.Registry.GetRules(eventType));

      // Execution

      if (template is not null)
      {
         _reflect.TemplateBundle(template.GenericType).Invoke(template.Instance, [context]);
      }

      foreach (var rule in _rules)
      {
         bool shouldApply = _reflect.RuleShouldApply(rule.GenericType).Invoke(rule, [context])!.As<bool>();
         if (!shouldApply) continue;
         var applied = _reflect.RuleApply(rule.GenericType).Invoke(rule, [context])!.As<IEvent>();
         _queue.Enqueue(applied);
      }

      if (contract is not null)
      {
         _validator.ValidateInputs(contract, context);
      }
      if (effect is not null)
      {
         _reflect.EffectApply(effect.GenericType).Invoke(effect, [@event, context]);
      }
      if (contract is not null)
      {
         _validator.ValidateOutputs(contract, context);
      }

      foreach (var rule in _rules)
      {
         bool shouldApply = _reflect.RuleShouldApply(rule.GenericType).Invoke(rule, [context])!.As<bool>();
         if (!shouldApply) continue;
         var applied = _reflect.RuleApply(rule.GenericType).Invoke(rule, [context])!.As<IEvent>();
         _queue.Enqueue(applied);
      }

      // Cleanup

      _rules.Clear();
   }
}