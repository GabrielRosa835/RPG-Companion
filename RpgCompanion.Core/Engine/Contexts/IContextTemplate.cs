using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IContextTemplate<in TEvent> where TEvent : IEvent
{
    void Bundle (TEvent @event, IContext context);
}