namespace RpgCompanion.Core;

public class PluginDescriptor
{
    public PluginKey Key { get; init; }
    public string? Name { get; init; }
    public string? Version { get; init; }
    public IReadOnlySet<EventKey> Events { get; init; } = default!;
    public IReadOnlySet<RuleKey> Rules { get; init; } = default!;
    public IReadOnlySet<ActorKey> Actors { get; init; } = default!;
}
