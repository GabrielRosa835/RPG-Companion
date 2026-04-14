namespace RpgCompanion.QuartzPrototype.Plugins;

using Descriptors;

// Singleton
internal class PluginCollection : List<PluginDescriptor>
{
    public PluginDescriptor this[string name] => FindByName(name);
    public PluginDescriptor this[EventKey eventKey] => FindByEventType(eventKey);

    private PluginDescriptor FindByName(string name)
    {
        return this.FirstOrDefault(d => d.Activated ? d.Identifier.Name == name : d.Resource == name)
               ?? throw new InvalidOperationException($"Plugin {name} not found");
    }

    private PluginDescriptor FindByEventType(EventKey eventKey)
    {
        return this.FirstOrDefault(d => d.Events.Contains(eventKey))
               ?? throw new InvalidOperationException($"Plugin for '{eventKey}' not found");
    }
}
