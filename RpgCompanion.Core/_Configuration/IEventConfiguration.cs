namespace RpgCompanion.Core;

public interface IEventConfiguration<TEvent> where TEvent : IEvent
{
    public IEventConfiguration<TEvent> WithName(string name);
    public IEventConfiguration<TEvent> WithPriority(int priority);
    public IEventConfiguration<TEvent> AddRule(Action<IRuleConfiguration<TEvent>> configure);
    public IEventConfiguration<TEvent> AddEffect(Action<IEffectConfiguration<TEvent>> configure);
}
