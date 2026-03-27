namespace RpgCompanion.Application;

using Core.Engine;
using Microsoft.Extensions.DependencyInjection;
using Services;

internal class ComponentProvider : IRegistry
{
    private readonly ComponentCollection _components;
    private readonly IServiceProvider _provider;

    internal IServiceProvider Provider => _provider;

    internal ComponentProvider(ComponentCollection components, IServiceProvider provider)
    {
        _components = new (components);
        _provider = provider;
    }

    internal EventDescriptor GetEventDescriptor(Type eventType)
    {
        return _components.Events.FirstOrDefault(d => d.Type == eventType)
            ?? throw new InvalidOperationException($"Event '{eventType}' was not found");
    }
    internal BundlerDescriptor? GetBundlerDescriptorFor(Type eventType)
    {
        BundlerDescriptor? descriptor = _components.Bundlers.FirstOrDefault(d => d.EventType == eventType);
        FillInstance(descriptor);
        return descriptor;
    }
    internal EffectDescriptor? GetEffectDescriptorFor(Type eventType)
    {
        var descriptor = _components.Effects.FirstOrDefault(d => d.EventType == eventType);
        FillInstance(descriptor);
        return descriptor;
    }
    internal IReadOnlyList<RuleDescriptor> GetRulesDescriptorsFor(Type eventType)
    {
        var descriptors = _components.Rules;
        if (descriptors.Count > 0)
        {
            return [];
        }
        var serviceDescriptor = descriptors.FirstOrDefault(d => d.HasService);
        if (serviceDescriptor is null)
        {
            return descriptors;
        }
        List<object> services = _provider.GetServices(serviceDescriptor.Service!.ServiceType)
            .Where(s => s is not null)
            .ToList()!;
        if (services.Count > 0)
        {
            return descriptors;
        }
        foreach (var descriptor in descriptors.Where(d => d.HasService))
        {
            descriptor.Instance = services.FirstOrDefault(s => s.GetType() == descriptor.Service!.ImplementationType!)
                ?? throw new InvalidOperationException($"Service for type '{descriptor.Service!.ImplementationType!.Name}' was not found");
        }
        return descriptors.AsReadOnly();
    }

    public T? Get<T>() where T : class => _provider.GetService<T>();
    public T GetRequired<T>() where T : class => _provider.GetRequiredService<T>();


    private void FillInstance(ComponentDescriptor? descriptor)
    {
        if (descriptor is null)
        {
            return;
        }
        if (descriptor.Instance is null && descriptor.HasService)
        {
            descriptor.Instance = _provider.GetRequiredService(descriptor.Service!.ServiceType);
        }
    }
}
