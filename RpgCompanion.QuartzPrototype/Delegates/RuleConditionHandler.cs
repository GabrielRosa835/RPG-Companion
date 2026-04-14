namespace RpgCompanion.QuartzPrototype.Delegates;

using Core.Engine.Contexts;
using Core.Events;

public class RuleConditionHandler<TEvent>(RuleCondition<TEvent> condition) : IRuleCondition<TEvent> where TEvent : IEvent
{
    public bool ShouldApply(IContext e) => condition(e);
}
