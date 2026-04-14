namespace RpgCompanion.Core.Events;

using Engine;

public delegate void EffectAction<in TEvent>(TEvent e, IPipeline pipeline) where TEvent : IEvent;
public delegate bool EffectCondition<in TEvent>(TEvent e) where TEvent : IEvent;

public interface IEffect;

public interface IEffect<in TEvent> : IEffect where TEvent : IEvent
{
    public void Apply(TEvent e);
}

public interface IEffectCondition<in TEvent> where TEvent : IEvent
{
    public bool ShouldApply(TEvent e);
}
