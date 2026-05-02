namespace RpgCompanion.Prototypes.MassTransit;

using Core;

public class Pipeline : IPipeline
{
    internal Queue<RuleKey> Transitions { get; } = [];

    public IPipeline Then<TEvent, TNext>(RuleKey<TEvent, TNext> transitionRuleKey)
    {
        Transitions.Enqueue(transitionRuleKey);
        return this;
    }
}
