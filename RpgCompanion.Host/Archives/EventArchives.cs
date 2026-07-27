namespace RpgCompanion.Host;

using System.Collections.Concurrent;

public class EventArchives
{
    private readonly ConcurrentDictionary<PluginKey, ConcurrentBag<EventDescriptor>> _entitiesByPlugin = new();
    private readonly ConcurrentDictionary<EventKey, EventDescriptor> _entitiesByKey = new();
    private readonly ConcurrentDictionary<Type, EventDescriptor> _entitiesByType = new();

    public EventDescriptor this[EventKey key] => _entitiesByKey[key];
    public EventDescriptor this[Type entityType] => _entitiesByType[entityType];
    public ConcurrentBag<EventDescriptor> this[PluginKey pluginKey] => _entitiesByPlugin[pluginKey];

    public void Add(EventDescriptor descriptor)
    {
        _entitiesByKey[descriptor.Key] = descriptor;
        _entitiesByType[descriptor.Type] = descriptor;
        if (!_entitiesByPlugin.TryGetValue(descriptor.PluginKey, out var entities))
        {
            entities = new ConcurrentBag<EventDescriptor>();
        }
        entities.Add(descriptor);
    }
}
