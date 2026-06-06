namespace RpgCompanion.Core;

public interface IRuleConfiguration<T, U>
{
    public IRuleConfiguration<T, U> WithKey(RuleKey<T, U> key);
    public IRuleConfiguration<T, U> WithName(string name);
    public IRuleConfiguration<T, U> WithDescription(string description);
    public IRuleConfiguration<T, U> WithOrder(double order);
    public IRuleConfiguration<T, U> WithCondition(Action<IConditionConfiguration<T>> configure);
    public IRuleConfiguration<T, U> Export(IRule<T, U> instance);
    public IRuleConfiguration<T, U> Export<TRule>() where TRule : class, IRule<T, U>;
}
