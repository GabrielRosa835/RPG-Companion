namespace RpgCompanion.Core;

public interface IEffectCondition<TEvent> where TEvent : IEvent
{
    public bool ShouldApply(TEvent e);
}

public delegate bool EffectCondition<TEvent>(TEvent e) where TEvent : IEvent;
