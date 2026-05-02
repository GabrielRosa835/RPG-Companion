namespace RpgCompanion.Core;

public class ActorDescriptor
{
    public ActorKey Key { get; init; }
    public ActorLifetime Lifetime { get; init; }
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public Type Type { get; init; } = default!;
    public ActorConnections Connections { get; init; } = default!;
}

public class ActorConnections
{
    public PluginKey Plugin { get; init; }
    public IReadOnlySet<RuleKey> Rules { get; init; } = default!;
    public IReadOnlySet<RuleKey> Actions { get; init; } = default!;
}
