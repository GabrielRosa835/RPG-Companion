namespace RpgCompanion.Host;

internal interface IPluginInitializer
{
    Task<List<InitializationResult>> InitializeMany(IEnumerable<LoadedPluginMetadata> plugins, CancellationToken cancellationToken = default);
    Task<InitializationResult> InitializeSingle(LoadedPluginMetadata metadata, CancellationToken cancellationToken = default);
}
