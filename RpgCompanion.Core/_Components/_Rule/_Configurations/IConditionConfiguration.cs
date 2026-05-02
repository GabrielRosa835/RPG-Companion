namespace RpgCompanion.Core;

public interface IConditionConfiguration<T>
{
    IConditionConfiguration<T> WithKey(RuleKey<T, bool> key);
    IConditionConfiguration<T> WithName(string name);
    IConditionConfiguration<T> WithDescription(string description);
    IConditionConfiguration<T> Export(Rule<T, bool> instance);
    IConditionConfiguration<T> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T, bool>;
}
