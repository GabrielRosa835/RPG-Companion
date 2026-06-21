using RpgCompanion.Core;

namespace RpgCompanion.Host;

internal class Trigger(
   IEventPublisher _publisher,
   IComponentGraph _components)
   : ITrigger
{
    public void Raise<TEvent>(TEvent e, Action<IPipeline<TEvent>>? pipeline = null) where TEvent : IEvent
    {
        var descriptor = _components.Events.Find<TEvent>();
        var transitions = new List<EventTransition>();
        if (pipeline is not null)
        {
            var pipelineBuilder = new Pipeline<TEvent>(transitions);
            pipeline.Invoke(pipelineBuilder);
        }
        var raising = new EventContext
        {
            Data = e,
            Descriptor = descriptor,
            Transitions = transitions,
        };
        _publisher.Publish(raising);
    }
}
