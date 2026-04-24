namespace RpgCompanion.Core;

public interface IPastEvents
{
    public TEvent? Find<TEvent>() where TEvent : IEvent;
    public TEvent? Find<TEvent>(Func<TEvent, bool> predicate) where TEvent : IEvent;
    public IEnumerable<TEvent> Search<TEvent>() where TEvent : IEvent;
    public IEnumerable<TEvent> Search<TEvent>(Func<TEvent, bool> predicate) where TEvent : IEvent;
}
