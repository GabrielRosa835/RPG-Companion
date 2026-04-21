namespace RpgCompanion.Host.Plugins;

using System.Reflection;

public class PluginMetadata
{
    internal string FilePath { get; init; }
    internal string Resource { get; init; }

    internal bool Activated { get; init; }
    internal Assembly Assembly { get; init; } = default!;
}
