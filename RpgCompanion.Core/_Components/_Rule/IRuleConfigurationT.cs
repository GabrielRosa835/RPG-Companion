namespace RpgCompanion.Core;

public interface IRuleConfiguration<T>
{
    public IRuleConfiguration<T> WithKey(RuleKey<T> key);
    public IRuleConfiguration<T> WithName(string name);
    public IRuleConfiguration<T> WithDescription(string description);
    public IRuleConfiguration<T> WithOrder(double order);
    public IRuleConfiguration<T> WithCondition(Action<IConditionConfiguration<T>> configure);
    public IRuleConfiguration<T> Export(IRule<T> instance);
    public IRuleConfiguration<T> Export<TRule>() where TRule : class, IRule<T>;
}
