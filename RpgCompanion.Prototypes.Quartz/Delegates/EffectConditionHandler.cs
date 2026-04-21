namespace RpgCompanion.QuartzPrototype.Delegates;

using Core.Events;

public class EffectConditionHandler<TEvent>(EffectCondition<TEvent> condition) : IEffectCondition<TEvent> where TEvent : IEvent
{
    public bool ShouldApply(TEvent e) => condition(e);
}
