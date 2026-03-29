namespace RpgCompanion.Application;

using Microsoft.Extensions.DependencyInjection;
using Services;

// Singleton
internal class PluginCollection : List<PluginDescriptor>
{
    public PluginDescriptor this[string name] => FindByName(name);
    public PluginDescriptor this[Type eventType] => FindByEventType(eventType);

    private PluginDescriptor FindByName(string name)
    {
        return this.FirstOrDefault(d => d.Activated ? d.Identifier.Name == name : d.Resource == name)
               ?? throw new InvalidOperationException($"Plugin {name} not found");
    }

    private PluginDescriptor FindByEventType(Type eventType)
    {
        return this.FirstOrDefault(d =>
               {
                   var components = d.Services?.GetService<ComponentCollection>();
                   return components is not null && components.Events.Any(e => e.Type == eventType);
               })
               ?? throw new InvalidOperationException($"Plugin for '{eventType}' not found");
    }
}
