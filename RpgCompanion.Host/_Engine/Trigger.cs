namespace RpgCompanion.Host;

using System.Text.Json;
using MassTransit;
using RpgCompanion.Core;

internal class Trigger(IBus _bus, IComponentGraph _components) : ITrigger
{
    public void Raise<TEvent>(TEvent e, System.Action<IPipeline<TEvent>>? pipeline = null) where TEvent : IEvent
    {
        var descriptor = _components.Events.Find<TEvent>();
        var transitions = new List<Transition>();
        if (pipeline is not null)
        {
            var pipelineBuilder = new Pipeline<TEvent>(transitions, _components);
            pipeline.Invoke(pipelineBuilder);
        }
        var raising = new EventRaisedEvent
        {
            Data = JsonSerializer.SerializeToElement(e, typeof(TEvent)),
            Key = descriptor.Key,
            Transitions = transitions
        };
        _bus.Publish(raising).GetAwaiter().GetResult();
    }
}
