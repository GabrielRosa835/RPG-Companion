namespace RpgCompanion.Core;

public interface ICondition<in T> : IRule<T, bool>
{
    public static ICondition<T> Of(Func<T, bool> condition) => new ConditionWrapper<T>(condition);
}

internal readonly record struct ConditionWrapper<T>(Func<T, bool> Delegate) : ICondition<T>
{
    public bool Apply(T target) => Delegate.Invoke(target);
}
