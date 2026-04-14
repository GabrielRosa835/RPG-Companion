namespace RpgCompanion.Core.Events;

public interface IEvent
{
    public static IEvent Empty => new EmptyEvent();
}

public record struct EmptyEvent : IEvent;
