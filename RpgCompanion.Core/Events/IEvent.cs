namespace RpgCompanion.Core.Events;

public interface IEvent
{
   string Name { get; }
   int Priority { get; }
}

public record EmptyEvent() : EventBase(nameof(EmptyEvent), 0);

public abstract record EventBase(string Name, int Priority) : IEvent;