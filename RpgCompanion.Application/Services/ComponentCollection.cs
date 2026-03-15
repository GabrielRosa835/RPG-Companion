namespace RpgCompanion.Application.Services;

using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Meta;

internal class ComponentCollection : IEnumerable<ComponentDescriptor>
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly List<ComponentDescriptor> _components = [];

    internal ComponentProvider BuildProvider()
    {
        return new ComponentProvider(_components, _services.BuildServiceProvider());
    }

    internal ComponentDescriptor AddComponent(ComponentDescriptor descriptor)
    {
        _components.Add(descriptor);
        return descriptor;
    }

    internal void AddInitializer<TInitializer>() where TInitializer : class, IInitializer
    {
        _services.AddTransient<TInitializer>();
    }
    internal EventDescriptor AddEvent<TEvent>()
       where TEvent : IEvent
    {
        var descriptor = new EventDescriptor
        {
            ComponentType = typeof(TEvent),
            GenericType = typeof(IEvent),
        };
        _components.Add(descriptor);
        return descriptor;
    }
    internal RuleDescriptor AddRule<TRule, TEvent>(RuleOrdering ordering)
       where TEvent : IEvent
       where TRule : IRule<TEvent>
    {
        var descriptor = new RuleDescriptor
        {
            ComponentType = typeof(TRule),
            GenericType = typeof(IRule<TEvent>),
            EventType = typeof(TEvent),
            Ordering = ordering,
        };
        _services.AddTransient(descriptor.ComponentType);
        _services.AddTransient(descriptor.GenericType, descriptor.ComponentType);
        _components.Add(descriptor);
        return descriptor;
    }
    internal EffectDescriptor AddEffect<TEffect, TEvent>()
       where TEvent : IEvent
       where TEffect : IEffect<TEvent>
    {
        var descriptor = new EffectDescriptor
        {
            ComponentType = typeof(TEffect),
            GenericType = typeof(IEffect<TEvent>),
            EventType = typeof(TEvent),
        };
        _services.AddTransient(descriptor.ComponentType);
        _services.AddTransient(descriptor.GenericType, descriptor.ComponentType);
        _components.Add(descriptor);
        return descriptor;
    }
    internal PackagerDescriptor AddPackager<TPackager, TEvent>()
       where TEvent : IEvent
       where TPackager : IPackager<TEvent>
    {
        var descriptor = new PackagerDescriptor
        {
            ComponentType = typeof(TPackager),
            GenericType = typeof(IPackager<TEvent>),
            EventType = typeof(TEvent),
        };
        _services.AddTransient(descriptor.ComponentType);
        _services.AddTransient(descriptor.GenericType, descriptor.ComponentType);
        _components.Add(descriptor);
        return descriptor;
    }

    public IEnumerator<ComponentDescriptor> GetEnumerator() => _components.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _components.GetEnumerator();
}
