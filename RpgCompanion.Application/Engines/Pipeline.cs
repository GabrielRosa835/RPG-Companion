namespace RpgCompanion.Application.Engines;

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using Services;

internal class Pipeline(ComponentProvider componentProvider, EventQueue eventQueue) : IPipeline
{
    IPipeline<TEvent> IPipeline.Raise<TEvent>(TEvent e)
    {
        var descriptor = componentProvider.GetEventDescriptor(e.GetType());
        var item = descriptor.CreateEvent(e);
        eventQueue.Enqueue(item);
        return new Pipeline<TEvent>(item, componentProvider, eventQueue);
    }
}

internal class Pipeline<TEvent>(EventItem item, ComponentProvider componentProvider, EventQueue eventQueue) : IPipeline<TEvent> where TEvent : IEvent
{
    public IPipeline<TEventOut> FollowedBy<TEventOut>(Continuation<TEvent, TEventOut> sequence) where TEventOut : IEvent
    {
        var descriptor = componentProvider.GetEventDescriptor(typeof(TEventOut));
        var newItem = descriptor.CreateEvent();
        item.Continuation = (sequence, newItem);
        eventQueue.Enqueue(newItem);
        return new Pipeline<TEventOut>(newItem, componentProvider, eventQueue);
    }
}
