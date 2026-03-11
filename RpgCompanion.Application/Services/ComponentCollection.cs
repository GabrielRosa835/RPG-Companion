using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Core.Meta;

using System.Collections;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application.Services;

internal class ComponentCollection : IEnumerable<ComponentDescriptor>
{
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

   internal ComponentDescriptor AddRule<TRule, TEvent> ()
      where TEvent : IEvent
      where TRule : IRule<TEvent>
   {
      var descriptor = new ComponentDescriptor
      {
         ComponentType = typeof(TRule),
         GenericType = typeof(IRule<TEvent>),
         EventType = typeof(TEvent),
         Category = ComponentCategory.Rule,
      };
      _services.AddTransient(descriptor.ComponentType);
      _services.AddTransient(descriptor.GenericType, descriptor.ComponentType);
      _components.Add(descriptor);
      return descriptor;
   }
   internal ComponentDescriptor AddEffect<TEffect, TEvent> ()
      where TEvent : IEvent
      where TEffect : IEffect<TEvent>
   {
      var descriptor = new ComponentDescriptor
      {
         ComponentType = typeof(TEffect),
         GenericType = typeof(IEffect<TEvent>),
         EventType = typeof(TEvent),
         Category = ComponentCategory.Effect,
      };
      _services.AddTransient(descriptor.ComponentType);
      _services.AddTransient(descriptor.GenericType, descriptor.ComponentType);
      _components.Add(descriptor);
      return descriptor;
   }
   internal ComponentDescriptor AddContract<TContract, TEvent> ()
      where TEvent : IEvent
      where TContract : IContract<TEvent>
   {
      var descriptor = new ComponentDescriptor
      {
         ComponentType = typeof(TContract),
         GenericType = typeof(IContract<TEvent>),
         EventType = typeof(TEvent),
         Category = ComponentCategory.Contract,
      };
      _services.AddTransient(descriptor.ComponentType);
      _services.AddTransient(descriptor.GenericType, descriptor.ComponentType);
      _components.Add(descriptor);
      return descriptor;
   }
   internal ComponentDescriptor AddPackager<TPackager, TEvent> ()
      where TEvent : IEvent
      where TPackager : IPackager<TEvent>
   {
      var descriptor = new ComponentDescriptor
      {
         ComponentType = typeof(TPackager),
         GenericType = typeof(IPackager<TEvent>),
         EventType = typeof(TEvent),
         Category = ComponentCategory.Packager,
      };
      _services.AddTransient(descriptor.ComponentType);
      _services.AddTransient(descriptor.GenericType, descriptor.ComponentType);
      _components.Add(descriptor);
      return descriptor;
   }

   public IEnumerator<ComponentDescriptor> GetEnumerator () => _components.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator () => _components.GetEnumerator();
}