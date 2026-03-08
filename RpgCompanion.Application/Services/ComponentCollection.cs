using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Core.Meta;

using System.Collections;

namespace RpgCompanion.Application.Services;

internal class ComponentCollection (IPlugin plugin) : IEnumerable<ComponentDescriptor>
{
   internal readonly IPlugin _plugin = plugin;

   internal readonly IServiceCollection _services = new ServiceCollection();
   internal readonly List<ComponentDescriptor> _components = [];

   internal ComponentProvider BuildProvider()
   {
      return new ComponentProvider(_components, _services.BuildServiceProvider());
   }

   internal ComponentDescriptor AddComponent (ComponentDescriptor descriptor)
   {
      _components.Add(descriptor);
      return descriptor;
   }

   internal ComponentDescriptor AddRule (
      Type ruleType,
      Type eventType,
      Type genericType,
      RulePlacement placement,
      object? instance = null)
   {
      var descriptor = new ComponentDescriptor
      {
         ComponentType = ruleType,
         GenericType = genericType,
         EventType = eventType,
         Category = ComponentCategory.Rule,
         Plugin = _plugin,
         Rule_Placement = placement,
      };
      if (instance is not null)
      {
         _services.AddSingleton(ruleType, instance);
         _services.AddTransient(genericType, s => s.GetRequiredService(ruleType));
      }
      else
      {
         _services.AddTransient(ruleType);
         _services.AddTransient(genericType, ruleType);
      }
      _components.Add(descriptor);
      return descriptor;
   }



   //internal ComponentDescriptor AddEffect<TEffect, TEvent> (object? instance = null, int? priority = null)
   //   where TEvent : IEvent
   //   where TEffect : IEffect<TEvent>
   //{
   //   return AddEffect(typeof(TEffect), typeof(TEvent), typeof(IEffect<TEvent>), instance, priority);
   //}
   //internal ComponentDescriptor AddEffect (
   //   Type effectType,
   //   Type eventType,
   //   Type genericType,
   //   object? instance = null,
   //   int? priority = null)
   //{
   //   var descriptor = new ComponentDescriptor
   //   {
   //      ComponentInstance = instance,
   //      ComponentType = effectType,
   //      GenericType = genericType,
   //      EventType = eventType,
   //      Plugin = _plugin,
   //      Category = ComponentCategory.Effect,
   //      Effect_Priority = priority ?? EffectPriority.None.Value(),
   //   };
   //   _services.AddTransient(effectType);
   //   _components.Add(descriptor);
   //   return descriptor;
   //}

   public IEnumerator<ComponentDescriptor> GetEnumerator () => _components.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator () => _components.GetEnumerator();
}

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddTransient(this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> implementationFactory)
   {
      services.Add(new ServiceDescriptor(
            serviceType,
            implementationFactory,
            ServiceLifetime.Transient));
      return services;
   }
}