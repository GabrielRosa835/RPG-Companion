namespace RpgCompanion.Core;

public interface IRule<T>
{
    T Apply(T target);
    public static IRule<T> Of(Func<T, T> rule) => new RuleWrapper<T>(rule);
}

internal readonly record struct RuleWrapper<T>(Func<T, T> Delegate) : IRule<T>
{
    public T Apply(T target) => Delegate(target);
}
