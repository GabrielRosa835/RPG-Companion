namespace RpgCompanion.Host;

using Core;
using MediatR;

public class Trigger(IMediator mediator, IComponentGraph components) : ITrigger
{
    public void Raise<TEvent>(TEvent e, Configure<IPipeline<TEvent>>? pipeline = null) where TEvent : IEvent
    {
        var nextEventType = typeof(TEvent);
        var descriptor = components.Events.FirstOrDefault(d => d.Type == nextEventType)
            ?? throw new InvalidOperationException($"Could not find a descriptor for event of type {nextEventType}");

        var transitions = new Queue<Func<IEvent, IEvent>>();
        var pipelineBuilder = new Pipeline<TEvent>(transitions);
        pipeline?.Invoke(pipelineBuilder);

        var raising = new EventRaisedEvent
        {
            Event = e,
            Descriptor = descriptor,
            Transitions = transitions,
        };

        // Sticking to your prototype's synchronous-over-async pattern for now
        mediator.Publish(raising).GetAwaiter().GetResult();
    }
}
