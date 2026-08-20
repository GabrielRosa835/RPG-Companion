namespace RpgCompanion.Core;

public class EventDescriptor
{
    public PluginKey PluginKey { get; init; }
    public EventKey Key { get; init; }
    public string? Name { get; init; }
    public Type Type { get; init; } = default!;
}
