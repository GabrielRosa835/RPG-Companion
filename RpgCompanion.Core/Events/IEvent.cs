namespace RpgCompanion.Core.Events;

public interface IEvent
{
   string Name { get; }
   public static readonly IEvent Empty = new EmptyEvent();
}

internal readonly record struct EmptyEvent : IEvent
{
   public string Name => nameof(EmptyEvent);
}

public abstract record EventBase(string Name) : IEvent;