namespace RpgCompanion.Application;

using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

internal class ComponentProvider : IRegistry
{
    private readonly IReadOnlyList<ComponentDescriptor> _components;
    private readonly IServiceProvider _provider;

    internal IServiceProvider Provider => _provider;

    internal ComponentProvider(IEnumerable<ComponentDescriptor> components, IServiceProvider provider)
    {
        _components = new List<ComponentDescriptor>(components).AsReadOnly();
        _provider = provider;
    }
    internal EventDescriptor GetEventDescriptor(IEvent @event)
    {
        var descriptor = GetEventDescriptor(@event.GetType());
        descriptor.Instance = @event;
        return descriptor;
    }
    internal EventDescriptor GetEventDescriptor(Type eventType)
    {
        return _components.First(d =>
           d is EventDescriptor ed &&
           ed.ComponentType == eventType)
           .As<EventDescriptor>();
    }
    internal PackagerDescriptor? GetPackagerDescriptorFor(Type eventType)
    {
        var descriptor = _components.FirstOrDefault(d =>
           d is PackagerDescriptor pd &&
           pd.EventType == eventType)?
           .As<PackagerDescriptor?>();
        if (descriptor is null)
            return null;
        var template = _provider.GetRequiredService(descriptor.GenericType);
        descriptor.Instance = template;
        return descriptor;
    }
    internal EffectDescriptor? GetEffectDescriptorFor(Type eventType)
    {
        var descriptor = _components.FirstOrDefault(d =>
           d is EffectDescriptor ed &&
           ed.EventType == eventType)?
           .As<EffectDescriptor?>();
        if (descriptor is null)
            return null;
        var effect = _provider.GetRequiredService(descriptor.GenericType);
        descriptor.Instance = effect;
        return (EffectDescriptor?) descriptor;
    }
    internal IEnumerable<RuleDescriptor> GetRulesDescriptorsFor(Type eventType)
    {
        var ruleDescriptors = _components.Where(d =>
           d is RuleDescriptor rd &&
           rd.EventType == eventType)
           .Cast<RuleDescriptor>()
           .ToArray();
        if (ruleDescriptors.Length > 0)
            return [];
        return _provider.GetServices(ruleDescriptors.First().GenericType)
           .Where(s => s is not null)
           .Select(s =>
           {
               var descriptor = ruleDescriptors.First(d => d.ComponentType == s!.GetType());
               descriptor.Instance = s!;
               return descriptor;
           })
           .ToList();
    }

    public T? GetComponent<T>() where T : class => _provider.GetService<T>();
    public T GetRequiredComponent<T>() where T : class => _provider.GetRequiredService<T>();
}
