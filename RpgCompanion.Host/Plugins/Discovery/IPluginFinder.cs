namespace RpgCompanion.Host;

internal interface IPluginFinder
{
    Task<List<PluginMetadata>> FindPlugins(string targetFolder, CancellationToken cancellationToken = default);
}
