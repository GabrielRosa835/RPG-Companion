namespace RpgCompanion.Application.Engines;

using Core.Engine;
using Core.Events;
using Services;

internal class Pipeline(EventQueue queue, ComponentProvider componentProvider) : IPipeline
{
    public IPipeline<TEvent> Raise<TEvent>(TEvent e) where TEvent : IEvent
    {
        var descriptor = componentProvider.GetEventDescriptor(e.GetType());
        var item = descriptor.CreateEvent(e);
        queue.Enqueue(item);
        return new Pipeline<TEvent>(item, componentProvider);
    }
}

internal class Pipeline<TEvent>(EventItem item, ComponentProvider componentProvider) : IPipeline<TEvent> where TEvent : IEvent
{
    public IPipeline<TEventOut> FollowedBy<TEventOut>(Continuation<TEvent, TEventOut> sequence) where TEventOut : IEvent
    {
        var descriptor = componentProvider.GetEventDescriptor(typeof(TEventOut));
        var newItem = descriptor.CreateEvent();
        item.Continuation = (sequence, newItem);
        return new Pipeline<TEventOut>(newItem, componentProvider);
    }
}
