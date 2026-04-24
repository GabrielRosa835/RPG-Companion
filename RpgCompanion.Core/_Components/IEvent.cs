namespace RpgCompanion.Core;

public interface IEvent
{
    public static IEvent Empty => new EmptyEvent();
}

public readonly record struct EmptyEvent : IEvent;
