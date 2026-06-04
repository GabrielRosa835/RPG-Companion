namespace RpgCompanion.Host;

using RpgCompanion.Core;

internal class Pipeline<TEvent>(List<Transition> _transitions, IComponentGraph _components) : IPipeline<TEvent> where TEvent : IEvent
{
    public IPipeline<TEvent> Then<TNext>(RuleKey<TEvent, TNext> transitionRuleKey, System.Action<IPipeline<TNext>>? pipeline = null)
        where TNext : IEvent
    {
        var transitions = new List<Transition>();
        if (pipeline is not null)
        {
            var pipelineBuilder = new Pipeline<TNext>(transitions, _components);
            pipeline.Invoke(pipelineBuilder);
        }
        var transition = new Transition
        {
            Key = transitionRuleKey,
            Chain = transitions,
        };
        _transitions.Add(transition);
        return this;
    }
}
