namespace RpgCompanion.Core;

public interface IEffect<T> : IRule<T>
{
    public static IEffect<T> Of(Func<T, T> effect) => new EffectWrapper<T>(effect);
}

internal readonly record struct EffectWrapper<T>(Func<T, T> Delegate) : IEffect<T>
{
    public T Apply(T target) => Delegate.Invoke(target);
}
