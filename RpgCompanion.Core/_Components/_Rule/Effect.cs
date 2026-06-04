namespace RpgCompanion.Core;

public record Effect<T>(Func<T, T> Delegate) : Rule<T>(Delegate), IEffect<T>;

public interface IEffect<T> : IRule<T>;

public static class Effect
{
    public static IEffect<T> Of<T>(Func<T, T> effect) => new Effect<T>(effect);
}
