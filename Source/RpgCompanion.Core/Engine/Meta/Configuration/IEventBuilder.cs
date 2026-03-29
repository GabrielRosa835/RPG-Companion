using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Meta;

public interface IEventBuilder<TEvent> where TEvent : IEvent
{
    public IEventBuilder<TEvent> WithName(string name);
    public IEventBuilder<TEvent> WithPriority(int priority);
    public IEventBuilder<TEvent> AddRule(Action<IRuleBuilder<TEvent>> configure);
    public IEventBuilder<TEvent> AddEffect(Action<IEffectBuilder<TEvent>> configure);
}
