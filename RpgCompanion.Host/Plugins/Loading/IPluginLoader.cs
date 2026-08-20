namespace RpgCompanion.Host;

internal interface IPluginLoader
{
    Task<List<LoadResult>> LoadMany(IEnumerable<PluginMetadata> plugins, CancellationToken cancellationToken = default);
    Task<LoadResult> LoadSingle(PluginMetadata metadata, CancellationToken cancellationToken = default);
}
