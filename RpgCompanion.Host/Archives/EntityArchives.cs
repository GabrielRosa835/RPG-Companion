namespace RpgCompanion.Host;

using System.Collections.Concurrent;

internal class EntityArchives
{
    private readonly ConcurrentDictionary<PluginKey, ConcurrentBag<EntityDescriptor>> _entitiesByPlugin = new();
    private readonly ConcurrentDictionary<EntityKey, EntityDescriptor> _entitiesByKey = new();
    private readonly ConcurrentDictionary<Type, EntityDescriptor> _entitiesByType = new();

    public EntityDescriptor this[EntityKey key] => _entitiesByKey[key];
    public EntityDescriptor this[Type entityType] => _entitiesByType[entityType];
    public ConcurrentBag<EntityDescriptor> this[PluginKey pluginKey] => _entitiesByPlugin[pluginKey];

    public void Add(EntityDescriptor descriptor)
    {
        _entitiesByKey[descriptor.Key] = descriptor;
        _entitiesByType[descriptor.Type] = descriptor;
        if (!_entitiesByPlugin.TryGetValue(descriptor.PluginKey, out var entities))
        {
            entities = new ConcurrentBag<EntityDescriptor>();
        }
        entities.Add(descriptor);
    }
}
