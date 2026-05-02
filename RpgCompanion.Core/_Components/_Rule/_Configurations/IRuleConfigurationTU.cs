namespace RpgCompanion.Core;

public interface IRuleConfiguration<T, U>
{
    public IRuleConfiguration<T, U> WithKey(RuleKey<T, U> key);
    public IRuleConfiguration<T, U> WithName(string name);
    public IRuleConfiguration<T, U> WithDescription(string description);
    public IRuleConfiguration<T, U> WithOrder(double order);
    public IRuleConfiguration<T, U> WithCondition(Configure<IConditionConfiguration<T>> configure);
    public IRuleConfiguration<T, U> Export(Rule<T, U> instance);
    public IRuleConfiguration<T, U> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T, U>;
}
