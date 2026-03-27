namespace RpgCompanion.Core.Engine;

using Events;

public interface ITrigger
{
    IPipeline<TEvent> Start<TEvent>(TEvent e) where TEvent : IEvent;
}
