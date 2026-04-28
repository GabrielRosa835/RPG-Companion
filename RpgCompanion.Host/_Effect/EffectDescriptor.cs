namespace RpgCompanion.Host;

using Core;

internal class EffectDescriptor
{
    public EffectKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public ConditionKey? Condition { get; init; }
    public EventKey? Event { get; init; }
    public ActorKey? Actor { get; init; }
    public string? DisplayName { get; init; }
}
