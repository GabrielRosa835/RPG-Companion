namespace RpgCompanion.Host;

using System.Collections.Concurrent;

public class IntentArchives
{
    private readonly ConcurrentDictionary<PluginKey, ConcurrentBag<IntentDescriptor>> _entitiesByPlugin = new();
    private readonly ConcurrentDictionary<IntentKey, IntentDescriptor> _entitiesByKey = new();
    private readonly ConcurrentDictionary<Type, IntentDescriptor> _entitiesByType = new();

    public IntentDescriptor this[IntentKey key] => _entitiesByKey[key];
    public IntentDescriptor this[Type entityType] => _entitiesByType[entityType];
    public ConcurrentBag<IntentDescriptor> this[PluginKey pluginKey] => _entitiesByPlugin[pluginKey];

    public void Add(IntentDescriptor descriptor)
    {
        _entitiesByKey[descriptor.Key] = descriptor;
        _entitiesByType[descriptor.Type] = descriptor;
        if (!_entitiesByPlugin.TryGetValue(descriptor.PluginKey, out var entities))
        {
            entities = new ConcurrentBag<IntentDescriptor>();
        }
        entities.Add(descriptor);
    }
}
