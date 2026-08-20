namespace RpgCompanion.Host;

internal class PluginManager(
    IPluginFinder _finder,
    IPluginLoader _loader,
    IPluginInitializer _initializer)
    : IPluginFinder, IPluginLoader, IPluginInitializer
{
    public Task<List<PluginMetadata>> FindPlugins(string targetFolder, CancellationToken cancellationToken = default)
    {
        return _finder.FindPlugins(targetFolder, cancellationToken);
    }

    public Task<List<LoadResult>> LoadMany(IEnumerable<PluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        return _loader.LoadMany(plugins, cancellationToken);
    }

    public Task<LoadResult> LoadSingle(PluginMetadata metadata, CancellationToken cancellationToken = default)
    {
        return _loader.LoadSingle(metadata, cancellationToken);
    }

    public Task<List<InitializationResult>> InitializeMany(IEnumerable<LoadedPluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        return _initializer.InitializeMany(plugins, cancellationToken);
    }

    public Task<InitializationResult> InitializeSingle(LoadedPluginMetadata metadata, CancellationToken cancellationToken = default)
    {
        return _initializer.InitializeSingle(metadata, cancellationToken);
    }
}
