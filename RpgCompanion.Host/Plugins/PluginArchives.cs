namespace RpgCompanion.Host;

using System.Collections.Concurrent;
using System.Reflection;

internal class PluginArchives
{
    private readonly ConcurrentDictionary<Assembly, PluginMetadata> _assemblyIndex = new();
    private readonly ConcurrentDictionary<MetadataId, PluginMetadata> _midIndex = new();
    private readonly ConcurrentDictionary<PluginId, PluginMetadata> _idIndex = new();

    internal PluginMetadata? this[string id] => _idIndex.GetValueOrDefault(new PluginId(id));
    internal PluginMetadata? this[Guid guid] => _midIndex.GetValueOrDefault(new MetadataId(guid));
    internal LoadedPluginMetadata? this[Assembly assembly] => _assemblyIndex.GetValueOrDefault(assembly) as LoadedPluginMetadata;

    internal void Upsert(PluginMetadata metadata)
    {
        if (metadata is LoadedPluginMetadata loaded)
        {
            Upsert(loaded);
            return;
        }

        var metadataId = new MetadataId(metadata.Id);
        _midIndex[metadataId] = metadata;

        var pluginId = new PluginId(metadata.Manifest.Id);
        _idIndex[pluginId] = metadata;
    }

    internal void Upsert(LoadedPluginMetadata metadata)
    {
        var id = new MetadataId(metadata.Id);

        var metadataId = new MetadataId(metadata.Id);
        _midIndex[metadataId] = metadata;

        var pluginId = new PluginId(metadata.Manifest.Id);
        _idIndex[pluginId] = metadata;

        _assemblyIndex[metadata.Assembly] = metadata;
    }

    internal void Upsert(InitializedPluginMetadata metadata)
    {
        Upsert(metadata as LoadedPluginMetadata);
    }

    private readonly record struct PluginId(string Value);

    private readonly record struct MetadataId(Guid Value);
}
