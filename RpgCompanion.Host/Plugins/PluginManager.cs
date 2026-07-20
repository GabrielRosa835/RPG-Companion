namespace RpgCompanion.Host;

using System.Collections.Concurrent;

internal class PluginManager(IEnumerable<PluginMetadata> plugins)
{
    public ConcurrentBag<PluginMetadata> Plugins { get; } = new(plugins);
}
