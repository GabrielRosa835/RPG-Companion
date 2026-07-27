namespace RpgCompanion.Host;

using System.Collections.Concurrent;

internal class PluginArchives
{
    private readonly ConcurrentDictionary<PluginKey, PluginMetadata> _pluginsByKey = new();

    internal PluginMetadata this[PluginKey key] => _pluginsByKey[key];

    internal void Add(PluginMetadata metadata)
    {
        if (!metadata.Loaded)
        {
            throw new InvalidOperationException("Plugin has not been loaded yet");
        }
        _pluginsByKey[metadata.Descriptor.Key] = metadata;
    }
}
