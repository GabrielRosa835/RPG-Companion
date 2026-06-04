namespace RpgCompanion.Core;

public interface IPipeline<TEvent> where TEvent : IEvent
{
    IPipeline<TEvent> Then<TNext>(RuleKey<TEvent, TNext> transitionRuleKey, System.Action<IPipeline<TNext>>? pipeline = null)
        where TNext : IEvent;
}
