namespace RpgCompanion.Host;

using Core;
using Plugins;

internal class PluginDescriptor
{
    internal PluginKey Key { get; init; }
    internal string? Name { get; init; }
    internal string? Version { get; init; }
    internal PluginMetadata Metadata { get; init; }

    internal IReadOnlySet<EventKey> Events { get; init; }
    internal IReadOnlySet<RuleKey> Rules { get; init; }
    internal IReadOnlySet<EffectKey> Effects { get; init; }
    internal IReadOnlySet<ConditionKey> Conditions { get; init; }
    internal IReadOnlySet<ActorKey> Actors { get; init; }
    internal InitializationKey? Initialization { get; init; }
}
