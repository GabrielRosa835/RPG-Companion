namespace RpgCompanion.Host;

using Core;

internal class RuleDescriptor
{
    public RuleKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public RuleKey? Condition { get; init; }
    public RuleKey? ConditionFor { get; init; }
    public EventKey? Event { get; init; }
    public ActorKey? Actor { get; init; }
    public string? DisplayName { get; init; }
}
