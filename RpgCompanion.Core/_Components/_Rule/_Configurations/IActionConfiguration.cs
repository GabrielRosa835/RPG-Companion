namespace RpgCompanion.Core;

public interface IActionConfiguration<T, TEvent> where TEvent : class, IEvent
{
    IActionConfiguration<T, TEvent> WithKey(RuleKey<T, TEvent> key);
    IActionConfiguration<T, TEvent> ForEvent(EventKey<TEvent> key);
    IActionConfiguration<T, TEvent> WithName(string name);
    IActionConfiguration<T, TEvent> WithDescription(string description);
    IActionConfiguration<T, TEvent> WithCondition(Configure<IConditionConfiguration<T>> configure);
    IActionConfiguration<T, TEvent> Export(Rule<T, TEvent> instance);
    IActionConfiguration<T, TEvent> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T, TEvent>;
}
