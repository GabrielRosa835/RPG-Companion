namespace RpgCompanion.Core;

public class EventDescriptor
{
    public EventKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public IReadOnlySet<RuleKey> Rules { get; init; } = default!;
    public string? DisplayName { get; init; }
    public int Priority { get; init; }
}
