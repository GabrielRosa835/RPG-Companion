namespace RpgCompanion.Application.Engines;

using Core.Engine;
using Core.Events;
using Services;

// Transient
internal class Pipeline(
    EventQueue queue, // Singleton
    ComponentProvider componentProvider // Transient
    ) : IPipeline
{
    public IPipeline<TEvent> Raise<TEvent>(TEvent e) where TEvent : IEvent
    {
        var descriptor = componentProvider.GetEventDescriptor(e.GetType());
        var item = descriptor.CreateItem(e);
        queue.Enqueue(item);
        return new EventPipeline<TEvent>(item, componentProvider);
    }
}

internal class EventPipeline<TEvent>(EventItem item, ComponentProvider componentProvider) : IPipeline<TEvent> where TEvent : IEvent
{
    public IPipeline<TEventOut> FollowedBy<TEventOut>(Continuation<TEvent, TEventOut> sequence) where TEventOut : IEvent
    {
        var descriptor = componentProvider.GetEventDescriptor(typeof(TEventOut));
        var newItem = descriptor.CreateItem();
        item.Continuation = new(sequence, newItem);
        return new EventPipeline<TEventOut>(newItem, componentProvider);
    }
}
