using RpgCompanion.Application.Engines;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Core.Engine;
using Utils.UnionTypes;

namespace RpgCompanion.Application;

internal class Engine
{
   private readonly PluginManager _pluginManager = default!;
   private readonly Reflect _reflect = default!;
   private readonly RuleCollection _rules = new();
   
   internal readonly EventQueue _queue = new();

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
      var context = new Context(this, plugin);
      var eventDesc = _queue.Dequeue();
      var templateDesc = plugin.Registry.GetPackagerDescriptorFor(eventDesc.ComponentType);
      var contractDesc = plugin.Registry.GetContractDescriptorFor(eventDesc.ComponentType);
      var effectDesc = plugin.Registry.GetEffectDescriptorFor(eventDesc.ComponentType);
      _rules.SetValues(plugin.Registry.GetRulesDescriptorsFor(eventDesc.ComponentType));

      // Packager-Pack
      if (templateDesc is not null)
      {
         _reflect.Packagers.Pack(templateDesc, eventDesc.Instance, context);
      }
      // Before-Rules
      foreach (var ruleDesc in _rules.AllBefore)
      {
         if (!_reflect.Rules.ShouldApply(ruleDesc, context)) continue;
         var event2 = _reflect.Rules.Apply(ruleDesc, context);
         var event2Desc = plugin.Registry.GetEventDescriptor(event2);
         _queue.Enqueue(event2Desc);
      }
      // Contract-Requirements
      if (contractDesc is not null)
      {
         foreach (var key in _reflect.Contracts.Requirements(contractDesc))
         {
            if (!context._data.Contains(key))
            {
               throw new ContractViolationException($"Event '{contractDesc.EventType.Name}' missing input: {key}");
            }
         }
      }
      // Effect-Apply
      if (effectDesc is not null)
      {
         _reflect.Effects.Apply(effectDesc, eventDesc.Instance, context);
      }
      // Contract-Outputs
      if (contractDesc is not null)
      {
         foreach (var key in _reflect.Contracts.Outputs(contractDesc))
         {
            if (!context._data.Contains(key))
            {
               throw new ContractViolationException($"Event '{contractDesc.EventType.Name}' failed to produce output: {key}");
            }
         }
      }
      // After-Rules
      foreach (var ruleDesc in _rules.AllAfter)
      {
         if (!_reflect.Rules.ShouldApply(ruleDesc, context)) continue;
         var event2 = _reflect.Rules.Apply(ruleDesc, context);
         var event2Desc = plugin.Registry.GetEventDescriptor(event2);
         _queue.Enqueue(event2Desc);
      }

      _rules.Clear();
   }
}