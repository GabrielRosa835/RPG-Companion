namespace RpgCompanion.Prototypes.MassTransit;

using Core;
using global::MassTransit;

public class Trigger(IBus bus, IComponentGraph components) : ITrigger
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
        bus.Publish(raising).GetAwaiter().GetResult();
    }
}
