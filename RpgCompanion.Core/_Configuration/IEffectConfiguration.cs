namespace RpgCompanion.Core;

public interface IEffectConfiguration<TEvent> where TEvent : IEvent
{
    IEffectConfiguration<TEvent> WithName(string name);

    IEffectConfiguration<TEvent> WithComponent<TEffect>() where TEffect : class, IEffect<TEvent>;
    IEffectConfiguration<TEvent> WithAction(EffectAction<TEvent> action);
    IEffectConfiguration<TEvent> WithCondition(EffectCondition<TEvent> condition);

    IEffectConfiguration<TEvent> WithComponentAsync<TEffect>() where TEffect : class, IAsyncEffect<TEvent>;
    IEffectConfiguration<TEvent> WithActionAsync(AsyncEffectAction<TEvent> action);
    IEffectConfiguration<TEvent> WithConditionAsync(AsyncEffectCondition<TEvent> condition);
}
