namespace RpgCompanion.Core.Meta;

using Application.Services;
using Events;
using Events.Producers;

public interface IRuleBuilder<TEvent> where TEvent : IEvent
{
    public IRuleBuilder<TEvent> WithOrdering(RuleOrdering ordering);
    public IRuleBuilder<TEvent> WithComponent<TRule>() where TRule : class, IRule<TEvent>;
    public IRuleBuilder<TEvent> WithAction(RuleAction<TEvent> action);
    public IRuleBuilder<TEvent> WithCondition(RuleCondition<TEvent> condition);
}
