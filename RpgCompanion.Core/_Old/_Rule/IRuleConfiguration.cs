namespace RpgCompanion.Events;

public interface IRuleConfiguration<T>
{
    public IRuleConfiguration<T> WithKey(RuleKey<T> key);
    public IRuleConfiguration<T> WithName(string name);
    public IRuleConfiguration<T> WithDescription(string description);
    public IRuleConfiguration<T> WithOrder(double order);
    public IRuleConfiguration<T> WithCondition(Action<IConditionConfiguration<T>> configure);
    public IRuleConfiguration<T> Export(Rule<T> rule);
}

public interface IRuleConfiguration<T, U>
{
    public IRuleConfiguration<T, U> WithKey(RuleKey<T, U> key);
    public IRuleConfiguration<T, U> WithName(string name);
    public IRuleConfiguration<T, U> WithDescription(string description);
    public IRuleConfiguration<T, U> WithOrder(double order);
    public IRuleConfiguration<T, U> WithCondition(Action<IConditionConfiguration<T>> configure);
    public IRuleConfiguration<T, U> Export(Rule<T, U> rule);
}
