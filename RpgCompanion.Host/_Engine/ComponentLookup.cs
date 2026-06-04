namespace RpgCompanion.Host;

using Core;

internal class ComponentLookup(IComponentGraph graph) : IComponentLookup
{
    public PluginDescriptor? Find(PluginKey key) => graph.Plugins
        .FirstOrDefault(p => p.Key == key);

    public EventDescriptor? Find(EventKey key) => graph.Events
        .FirstOrDefault(e => e.Key == key);

    public ActorDescriptor? Find(ActorKey key) => graph.Actors
        .FirstOrDefault(a => a.Key == key);

    public RuleDescriptor? Find(RuleKey key) => graph.Rules
        .FirstOrDefault(r => r.Key == key);

    public PluginDescriptor Get(PluginKey key) => Find(key)
        ?? throw new InvalidOperationException();

    public EventDescriptor Get(EventKey key) => Find(key)
        ?? throw new InvalidOperationException();

    public ActorDescriptor Get(ActorKey key) => Find(key)
        ?? throw new InvalidOperationException();

    public RuleDescriptor Get(RuleKey key) => Find(key)
        ?? throw new InvalidOperationException();
}
