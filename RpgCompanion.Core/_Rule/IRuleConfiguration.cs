namespace RpgCompanion.Core;

public interface IRuleConfiguration<T>
{
    public IRuleConfiguration<T> WithName(string name);
    public IRuleConfiguration<T> WithCondition(Action<IConditionConfiguration<T>> configure);
    public IRuleConfiguration<T> Export(Rule<T> instance);
    public IRuleConfiguration<T> Export(Func<IRegistry, RuleKey, Rule<T>> factory);
}
