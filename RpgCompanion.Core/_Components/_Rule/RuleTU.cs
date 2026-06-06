namespace RpgCompanion.Core;

public interface IRule<in T, out U>
{
    U Apply(T target);
    public static IRule<T, U> Of(Func<T, U> rule) => new RuleWrapper<T, U>(rule);
}

internal readonly record struct RuleWrapper<T, U>(Func<T, U> Delegate) : IRule<T, U>
{
    public U Apply(T target) => Delegate(target);
}
