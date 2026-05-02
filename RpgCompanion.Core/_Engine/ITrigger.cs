namespace RpgCompanion.Core;

public interface ITrigger
{
    void Raise<TEvent>(TEvent e, Configure<IPipeline<TEvent>>? pipeline = null) where TEvent : IEvent;
}
