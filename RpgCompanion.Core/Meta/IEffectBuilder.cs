namespace RpgCompanion.Core.Meta;

using Events;

public interface IEffectBuilder<TEvent> where TEvent : IEvent
{
    public IEffectBuilder<TEvent> WithComponent<TEffect>() where TEffect : class, IEffect<TEvent>;
    public IEffectBuilder<TEvent> WithAction(EffectAction<TEvent> action);
    public IEffectBuilder<TEvent> WithCondition(EffectCondition<TEvent> condition);
}
