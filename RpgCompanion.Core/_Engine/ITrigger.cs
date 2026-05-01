namespace RpgCompanion.Core;


public interface ITrigger
{
    public IPipeline<TEvent> Raise<TEvent>(TEvent e) where TEvent : IEvent;
}
