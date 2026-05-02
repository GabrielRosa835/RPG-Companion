namespace RpgCompanion.Core;

public interface IEvent
{
    public static IEvent Empty => new EmptyEvent();
}
