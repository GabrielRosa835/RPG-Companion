namespace RpgCompanion.Host;

using System.Collections.Concurrent;

internal class PluginCache : ConcurrentBag<PluginMetadata>;
