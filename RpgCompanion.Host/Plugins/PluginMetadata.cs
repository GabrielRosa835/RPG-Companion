namespace RpgCompanion.Host;

using System.Reflection;

internal class PluginMetadata
{
    internal string FilePath { get; init; }
    internal string Resource { get; init; }

    internal bool Loaded { get; set; }
    internal Assembly Assembly { get; set; } = default!;

    internal bool Initialized { get; set; }
    internal PluginDescriptor Descriptor { get; set; } = default!;
    internal IServiceProvider Services { get; set; } = default!;

    public PluginMetadata(string filePath)
    {
        FilePath = filePath;
        Resource = Path.GetFileNameWithoutExtension(FilePath);
    }
}
