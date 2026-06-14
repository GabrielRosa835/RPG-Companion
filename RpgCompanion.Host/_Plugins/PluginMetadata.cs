namespace RpgCompanion.Host;

using System.Reflection;
using Core;

internal class PluginMetadata
{
    internal string FilePath { get; init; }
    internal string Resource { get; init; }

    internal bool Activated { get; set; }
    internal Assembly Assembly { get; set; } = default!;

    internal bool Initialized { get; set; }
    internal Initialization? Initialization { get; set; }
    internal PluginDescriptor Descriptor { get; set; } = default!;

    public PluginMetadata(string filePath)
    {
        FilePath = filePath;
        Resource = Path.GetFileNameWithoutExtension(FilePath);
    }
}
