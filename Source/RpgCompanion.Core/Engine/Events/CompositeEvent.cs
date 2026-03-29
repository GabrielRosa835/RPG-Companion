namespace RpgCompanion.Core.Events;

using Engine;

internal record CompositeEvent(params IEvent[] Events) : IEvent;

internal class CompositeEffect : IEffect<CompositeEvent>
{
    public bool ShouldApply(CompositeEvent @event)
    {
        return true;
    }
    public void Apply(CompositeEvent @event, IPipeline pipeline)
    {
        foreach (var inner in @event.Events)
        {
            pipeline.Raise(inner);
        }
    }
}
