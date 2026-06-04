namespace RpgCompanion.Core;

public interface ITrigger
{
    void Raise<TEvent>(TEvent e, System.Action<IPipeline<TEvent>>? pipeline = null) where TEvent : IEvent;
}
