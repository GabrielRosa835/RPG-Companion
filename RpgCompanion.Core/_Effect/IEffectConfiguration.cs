namespace RpgCompanion.Core;

public interface IEffectConfiguration<T>
{
    public IEffectConfiguration<T> WithName(string name);
    public IEffectConfiguration<T> WithCondition(Action<IConditionConfiguration<T>> configure);
    public IEffectConfiguration<T> Export<TResult>(Effect<T, TResult> instance);
    public IEffectConfiguration<T> Export<TResult>(Func<IRegistry, EffectKey, Effect<T, TResult>> factory);
}
