namespace RpgCompanion.Core;

public class RuleDescriptor
{
    public RuleKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public RuleKey? Condition { get; init; }
    public EventKey? Event { get; init; }
    public ActorKey? Actor { get; init; }
    public string? DisplayName { get; init; }
}
