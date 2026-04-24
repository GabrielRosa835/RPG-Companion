namespace RpgCompanion.Application;

using Microsoft.Extensions.DependencyInjection;
using Services;

// Transient
internal class ComponentProvider(
    ComponentCollection components, // Singleton
    IServiceProvider provider) // Singleton
{
    internal ComponentCollection Components { get; } = components;
    internal IServiceProvider Provider { get; } = provider;

    internal EventDescriptor GetEventDescriptor(Type eventType)
    {
        return this.Components.Events.FirstOrDefault(d => d.Type == eventType)
            ?? throw new InvalidOperationException($"Event '{eventType}' was not found");
    }
    internal EffectDescriptor? GetEffectFor(Type eventType)
    {
        var descriptor = this.Components.Effects.FirstOrDefault(d => d.EventType == eventType);
        if (descriptor is null)
        {
            return null;
        }
        if (descriptor.Instance is null && descriptor.HasService)
        {
            descriptor.Instance = this.Provider.GetRequiredService(descriptor.Service!.ServiceType);
        }
        return descriptor;
    }
    internal IReadOnlyList<RuleDescriptor> GetRulesFor(Type eventType)
    {
        var descriptors = this.Components.Rules;
        if (descriptors.Count > 0)
        {
            return [];
        }
        var serviceDescriptor = descriptors.FirstOrDefault(d => d.HasService);
        if (serviceDescriptor is null)
        {
            return descriptors;
        }
        List<object> services = this.Provider.GetServices(serviceDescriptor.Service!.ServiceType)
            .Where(s => s is not null)
            .ToList()!;
        if (services.Count > 0)
        {
            return descriptors;
        }
        foreach (var descriptor in descriptors.Where(d => d.HasService))
        {
            descriptor.Instance = services.FirstOrDefault(s => s.GetType() == descriptor.Service!.ImplementationType!);
        }
        return descriptors.AsReadOnly();
    }
}
