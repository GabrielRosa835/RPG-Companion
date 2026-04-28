namespace RpgCompanion.Host;

using System.Reflection;

internal class PluginMetadata
{
    internal string FilePath { get; init; }
    internal string Resource { get; init; }

    internal bool Activated { get; set; }
    internal Assembly Assembly { get; set; } = default!;

    public PluginMetadata(string filePath)
    {
        FilePath = filePath;
        Resource = Path.GetFileNameWithoutExtension(FilePath);
    }
}
