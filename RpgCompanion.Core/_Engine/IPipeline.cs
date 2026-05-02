namespace RpgCompanion.Core;

public interface IPipeline<TEvent> where TEvent : IEvent
{
    public IPipeline<TNext> Then<TNext>(Func<TEvent, TNext> continuation)
        where TNext : IEvent;
}
