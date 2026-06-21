using RpgCompanion.Core;

namespace RpgCompanion.Host;

internal class Pipeline<TEvent>(List<EventTransition> _transitions)
    : IPipeline<TEvent> where TEvent : IEvent
{
    public IPipeline<TEvent> Then<TNext>(
        Rule<TEvent, TNext> transition,
        Action<IPipeline<TNext>>? pipeline = null)
        where TNext : IEvent
    {
        var transitions = new List<EventTransition>();
        if (pipeline is not null)
        {
            var pipelineBuilder = new Pipeline<TNext>(transitions);
            pipeline.Invoke(pipelineBuilder);
        }
        var eventTransition = new EventTransition
        {
            Rule = (e, ctx) => transition((TEvent) e, ctx),
            Chain = transitions,
        };
        _transitions.Add(eventTransition);
        return this;
    }
}
