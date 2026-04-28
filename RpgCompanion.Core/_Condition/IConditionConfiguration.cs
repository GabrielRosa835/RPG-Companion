namespace RpgCompanion.Core;

public interface IConditionConfiguration<T>
{
    public IConditionConfiguration<T> WithName(string name);
    public IConditionConfiguration<T> Export(Condition<T> instance);
    public IConditionConfiguration<T> Export(Func<IRegistry, ConditionKey, Condition<T>> factory);
}
