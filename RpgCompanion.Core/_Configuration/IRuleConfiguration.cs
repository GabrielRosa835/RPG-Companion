namespace RpgCompanion.Core;

public interface IRuleConfiguration<TEvent> where TEvent : IEvent
{
    IRuleConfiguration<TEvent> WithName(string name);
    IRuleConfiguration<TEvent> WithOrdering(RuleOrdering ordering);

    IRuleConfiguration<TEvent> WithComponent<TRule>() where TRule : class, IRule<TEvent>;
    IRuleConfiguration<TEvent> WithAction(RuleAction<TEvent> action);
    IRuleConfiguration<TEvent> WithCondition(RuleCondition condition);

    IRuleConfiguration<TEvent> WithComponentAsync<TRule>() where TRule : class, IAsyncRule<TEvent>;
    IRuleConfiguration<TEvent> WithActionAsync(AsyncRuleAction<TEvent> action);
    IRuleConfiguration<TEvent> WithConditionAsync(AsyncRuleCondition condition);
}
