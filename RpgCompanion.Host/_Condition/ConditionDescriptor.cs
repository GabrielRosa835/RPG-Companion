namespace RpgCompanion.Host;

using Core;

public class ConditionDescriptor
{
    public ConditionKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public EffectKey? Effect { get; init; }
    public RuleKey? Rule { get; init; }
}
