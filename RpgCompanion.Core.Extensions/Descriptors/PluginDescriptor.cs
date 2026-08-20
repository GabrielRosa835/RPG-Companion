namespace RpgCompanion.Core;

public class PluginDescriptor
{
    public PluginKey Key { get; init; }

    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string EntryPoint { get; init; } = string.Empty;
    public string PdkVersion { get; init; } = string.Empty;
}
