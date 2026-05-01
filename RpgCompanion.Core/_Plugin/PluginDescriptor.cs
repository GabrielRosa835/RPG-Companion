namespace RpgCompanion.Core;

public class PluginDescriptor
{
    public PluginKey Key { get; init; }
    public string? Name { get; init; }
    public string? Version { get; init; }
    public IReadOnlySet<EventKey> Events { get; init; }
    public IReadOnlySet<RuleKey> Rules { get; init; }
    public IReadOnlySet<ActorKey> Actors { get; init; }
}
