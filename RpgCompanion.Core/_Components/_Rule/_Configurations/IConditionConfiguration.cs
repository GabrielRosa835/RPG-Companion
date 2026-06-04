namespace RpgCompanion.Core;

public interface IConditionConfiguration<T>
{
    IConditionConfiguration<T> WithKey(RuleKey<T, bool> key);
    IConditionConfiguration<T> WithName(string name);
    IConditionConfiguration<T> WithDescription(string description);
    IConditionConfiguration<T> Export(IRule<T, bool> instance);
    IConditionConfiguration<T> Export<TRule>() where TRule : class, IRule<T, bool>;
}
