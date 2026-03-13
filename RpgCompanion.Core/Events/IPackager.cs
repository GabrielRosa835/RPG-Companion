using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Contexts;

public interface IPackager<in TEvent> where TEvent : IEvent
{
    void Pack (TEvent @event, IContext context);
}