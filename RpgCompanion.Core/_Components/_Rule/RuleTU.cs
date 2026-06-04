namespace RpgCompanion.Core;

public interface IRule<in T, out U>
{
    U Apply(T target);
}

public record Rule<T, U>(Func<T, U> Delegate) : IRule<T, U>
{
    public U Apply(T target) => Delegate(target);

    public static implicit operator Rule<T, U>(U value) => new(_ => value);
    public static implicit operator Rule<T, U>(Func<T, U> rule) => new(rule);
    public static implicit operator Func<T, U>(Rule<T, U> rule) => rule.Delegate;
}
