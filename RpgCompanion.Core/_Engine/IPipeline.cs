namespace RpgCompanion.Core;

public interface IPipeline<out TEvent> where TEvent : IEvent
{
    IPipeline<TEvent> Then<TNext>(Rule<TEvent, TNext> transition, Action<IPipeline<TNext>>? pipeline = null)
        where TNext : IEvent;
}
