namespace RpgCompanion.Host;

using Core;

public class ComponentGraph(
    IEnumerable<PluginDescriptor> _plugins,
    IEnumerable<EventDescriptor> _events,
    IEnumerable<ActorDescriptor> _actors,
    IEnumerable<RuleDescriptor> _rules)
    : IComponentGraph
{
    // TODO: Improve performance of this
    public IQueryable<PluginDescriptor> Plugins => _plugins.AsQueryable();
    public IQueryable<EventDescriptor> Events => _events.AsQueryable();
    public IQueryable<ActorDescriptor> Actors => _actors.AsQueryable();
    public IQueryable<RuleDescriptor> Rules => _rules.AsQueryable();
}
