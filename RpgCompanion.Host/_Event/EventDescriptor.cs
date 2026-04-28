namespace RpgCompanion.Host;

using Core;

internal class EventDescriptor
{
    public EventKey Key { get; init; }
    public PluginKey Plugin { get; init; }
    public IReadOnlySet<EffectKey> Effects { get; init; } = default!;
    public IReadOnlySet<RuleKey> Rules { get; init; } = default!;
    public string? DisplayName { get; init; }
    public int Priority { get; init; }
}
