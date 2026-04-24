namespace RpgCompanion.Application.Engine.Contexts;

using Core.Events;

internal class PastEventsCollection : Queue<IEvent>, IPastEvents
{
    public TEvent? Find<TEvent>() where TEvent : IEvent
    {
        return this.OfType<TEvent>().FirstOrDefault();
    }
    public TEvent? Find<TEvent>(Func<TEvent, bool> predicate) where TEvent : IEvent
    {
        return this.OfType<TEvent>().FirstOrDefault(predicate);
    }
    public IEnumerable<TEvent> Search<TEvent>() where TEvent : IEvent
    {
        return this.OfType<TEvent>();
    }
    public IEnumerable<TEvent> Search<TEvent>(Func<TEvent, bool> predicate) where TEvent : IEvent
    {
        return this.OfType<TEvent>().Where(predicate);
    }
}
