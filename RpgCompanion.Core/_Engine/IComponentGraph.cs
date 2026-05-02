namespace RpgCompanion.Core;

public interface IComponentGraph
{
    IQueryable<PluginDescriptor> Plugins { get; }
    IQueryable<EventDescriptor> Events { get; }
    IQueryable<ActorDescriptor> Actors { get; }
    IQueryable<RuleDescriptor> Rules { get; }
}
