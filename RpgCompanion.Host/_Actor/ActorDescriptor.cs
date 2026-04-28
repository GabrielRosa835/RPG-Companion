namespace RpgCompanion.Host;

using Core;

public class ActorDescriptor
{
    public ActorKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public IReadOnlySet<RuleKey> Rules { get; init; }
    public IReadOnlySet<EffectKey> Effects { get; init; }
    public string? DisplayName { get; init; }
}
