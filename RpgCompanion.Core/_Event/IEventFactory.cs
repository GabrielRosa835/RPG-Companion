namespace RpgCompanion.Core;

public interface IEventFactory
{
    TEvent Create<TEvent>() where TEvent : Event;
    Event Create(Type eventType);
}
