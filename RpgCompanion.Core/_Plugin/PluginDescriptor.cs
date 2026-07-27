namespace RpgCompanion.Core;

public class PluginDescriptor
{
    public PluginKey Key { get; init; } = default!;
    public string Identifier { get; init; } = default!;
    public string? Name { get; init; }
    public string? Version { get; init; }
}
