namespace RpgCompanion.Events;

public interface IActionConfiguration<T, TEvent> where TEvent : class, IEvent
{
    IActionConfiguration<T, TEvent> WithKey(RuleKey<T, TEvent> key);
    IActionConfiguration<T, TEvent> ForEvent(EventKey<TEvent> key);
    IActionConfiguration<T, TEvent> WithName(string name);
    IActionConfiguration<T, TEvent> WithDescription(string description);
    IActionConfiguration<T, TEvent> WithOrder(double order);
    IActionConfiguration<T, TEvent> WithCondition(Action<IConditionConfiguration<T>> configure);
    IActionConfiguration<T, TEvent> Export(Rule<T, TEvent> rule);
}
