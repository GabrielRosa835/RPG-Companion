namespace RpgCompanion.Core;

public class ActorDescriptor
{
    public ActorKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public IReadOnlySet<RuleKey> Rules { get; init; }
    public string? DisplayName { get; init; }
}
