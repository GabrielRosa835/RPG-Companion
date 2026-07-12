namespace RpgCompanion.Events;

public class RuleDescriptor
{
    public RuleKey Key { get; init; }
    public double Order { get; init; }
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public PluginKey Plugin { get; init; }
    public IReadOnlySet<RuleKey> Conditions { get; init; } = default!;
    public RuleKey? ForRule { get; init; }
    public EventKey? ForEvent { get; init; }
    public EventKey? Event { get; init; }
    public ActorKey? Actor { get; init; }
}
