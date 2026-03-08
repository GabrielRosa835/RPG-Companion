using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Contexts;

public interface IContextTemplate<in TEvent> where TEvent : IEvent
{
    void Bundle (TEvent @event, IContext context);
}