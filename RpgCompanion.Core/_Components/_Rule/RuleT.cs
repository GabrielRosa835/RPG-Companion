namespace RpgCompanion.Core;

public interface IRule<T>
{
    T Apply(T target);
}

public record Rule<T>(Func<T, T> Delegate) : IRule<T>
{
    public T Apply(T target) => Delegate(target);

    public static implicit operator Rule<T>(T value) => new(_ => value);
    public static implicit operator Rule<T>(Func<T, T> rule) => new(rule);
    public static implicit operator Func<T, T>(Rule<T> rule) => rule.Delegate;
}
