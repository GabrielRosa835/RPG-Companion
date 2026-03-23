namespace RpgCompanion.Core.Events;

using RpgCompanion.Core.Engine;

public delegate void EffectAction<in TEvent>(TEvent e, IPipeline pipeline) where TEvent : IEvent;
public delegate bool EffectCondition<in TEvent>(TEvent e) where TEvent : IEvent;

public interface IEffect<in TEvent> where TEvent : IEvent
{
    public bool ShouldApply(TEvent e);
    public void Apply(TEvent e, IPipeline pipeline);
}
