namespace RpgCompanion.Events;

public class EventDescriptor
{
    // public EventKey Key { get; init; }
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public Type Type { get; init; } = default!;
    public PluginKey Plugin { get; init; }
}
