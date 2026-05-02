namespace RpgCompanion.Core;

public interface IRuleConfiguration<T>
{
    public IRuleConfiguration<T> WithKey(RuleKey<T> key);
    public IRuleConfiguration<T> WithName(string name);
    public IRuleConfiguration<T> WithDescription(string description);
    public IRuleConfiguration<T> WithOrder(double order);
    public IRuleConfiguration<T> WithCondition(Configure<IConditionConfiguration<T>> configure);
    public IRuleConfiguration<T> Export(Rule<T> instance);
    public IRuleConfiguration<T> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T>;
}
