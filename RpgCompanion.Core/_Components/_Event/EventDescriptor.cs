namespace RpgCompanion.Core;

public class EventDescriptor
{
    public EventKey Key { get; init; }
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public Type Type { get; init; } = default!;
    public EventConnections Connections { get; init; } = default!;
}

public class EventConnections
{
    public PluginKey Plugin { get; init; }
    public IReadOnlySet<RuleKey> Rules { get; init; } = default!;
    public IReadOnlySet<RuleKey> Actions { get; init; } = default!;
}
