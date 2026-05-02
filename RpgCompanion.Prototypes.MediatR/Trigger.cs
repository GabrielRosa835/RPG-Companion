namespace RpgCompanion.Prototypes.MediatR;

using Core;
using global::MediatR;

public class Trigger(IMediator mediator, IComponentGraph components) : ITrigger
{
    public void Raise<TEvent>(TEvent e, Action<IPipeline>? pipeline = null) where TEvent : IEvent
    {
        var nextEventType = typeof(TEvent);
        var descriptor = components.Events.FirstOrDefault(d => d.Type == nextEventType)
            ?? throw new InvalidOperationException($"Could not find a descriptor for event of type {nextEventType}");

        var pipelineBuilder = new Pipeline();
        pipeline?.Invoke(pipelineBuilder);

        var raising = new EventRaisedEvent<TEvent>
        {
            Event = e,
            DescriptorKey = descriptor.Key,
            Transitions = pipelineBuilder.Transitions
        };

        // Sticking to your prototype's synchronous-over-async pattern for now
        mediator.Publish(raising).GetAwaiter().GetResult();
    }
}
