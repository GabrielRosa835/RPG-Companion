namespace RpgCompanion.Core.Events;

using RpgCompanion.Core.Engine;

public interface IEffect<in TEvent> where TEvent : IEvent
{
    public bool ShouldApply(TEvent e);
    public void Apply(TEvent e, IPipeline pipeline);
}
