namespace RpgCompanion.Application.Engines;

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

internal class Pipeline(ComponentProvider componentProvider, EventQueue eventQueue) : IPipeline
{
    private readonly ComponentProvider _componentProvider = componentProvider;
    private readonly EventQueue _eventQueue = eventQueue;

    IPipeline<TEvent> IPipeline.Raise<TEvent>(TEvent @event)
    {
        var descriptor = _componentProvider.GetEventDescriptor(@event);
        _eventQueue.Enqueue(descriptor);
        return new Pipeline<TEvent>(_componentProvider, _eventQueue);
    }
}

internal class Pipeline<TEvent>(ComponentProvider componentProvider, EventQueue eventQueue) : IPipeline<TEvent> where TEvent : IEvent
{
    private readonly ComponentProvider _componentProvider = componentProvider;
    private readonly EventQueue _eventQueue = eventQueue;

    public IPipeline<TEventOut> FollowedBy<TEventOut>(Func<TEvent, TEventOut> sequence) where TEventOut : IEvent
    {
        var descriptor = _componentProvider.GetEventDescriptor(typeof(TEvent));
        descriptor.Continuations.Add(sequence);
        return new Pipeline<TEventOut>(_componentProvider, _eventQueue);
    }
}
