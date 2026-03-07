using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Meta;

using System.Collections;

namespace RpgCompanion.Application.Services;

internal class ComponentCollection (IPlugin plugin) : IEnumerable<ComponentDescriptor>
{
   internal readonly IPlugin _plugin = plugin;

   internal readonly IServiceCollection _services = new ServiceCollection();
   internal readonly List<ComponentDescriptor> _components = new();

   internal ComponentProvider BuildProvider()
   {
      return new ComponentProvider(_components, _services.BuildServiceProvider());
   }


   internal ComponentDescriptor AddComponent (ComponentDescriptor descriptor)
   {
      _components.Add(descriptor);
      return descriptor;
   }

   internal ComponentDescriptor AddRule<TRule, TEvent> (object? instance = null, RulePlacement? placement = null) 
      where TEvent : IEvent
      where TRule : IRule<TEvent>
   {
      return AddRule(typeof(TRule), typeof(TEvent), instance, placement);
   }
   internal ComponentDescriptor AddRule (Type ruleType, Type eventType, object? instance = null, RulePlacement? placement = null)
   {
      var descriptor = new ComponentDescriptor
      {
         ComponentInstance = instance,
         ComponentType = ruleType,
         EventType = eventType,
         Category = ComponentCategory.Rule,
         Plugin = _plugin,
         Rule_Placement = placement ?? RulePlacement.None,
      };
      _services.AddTransient(ruleType);
      _components.Add(descriptor);
      return descriptor;
   }



   internal ComponentDescriptor AddEffect<TEffect, TEvent> (object? instance = null, int? priority = null)
      where TEvent : IEvent
      where TEffect : IEffect<TEvent>
   {
      return AddEffect(typeof(TEffect), typeof(TEvent), instance, priority);
   }
   internal ComponentDescriptor AddEffect (Type effectType, Type eventType, object? instance = null, int? priority = null)
   {
      var descriptor = new ComponentDescriptor
      {
         ComponentInstance = instance,
         ComponentType = effectType,
         EventType = eventType,
         Plugin = _plugin,
         Category = ComponentCategory.Effect,
         Effect_Priority = priority ?? EffectPriority.None.Value(),
      };
      _services.AddTransient(effectType);
      _components.Add(descriptor);
      return descriptor;
   }

   public IEnumerator<ComponentDescriptor> GetEnumerator () => _components.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator () => _components.GetEnumerator();
}
