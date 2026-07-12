namespace RpgCompanion.Events;

public interface IEventConfiguration<TEvent> where TEvent : class, IEvent
{
    public IEventConfiguration<TEvent> WithKey(EventKey<TEvent> key);
    public IEventConfiguration<TEvent> WithName(string name);
    public IEventConfiguration<TEvent> WithDescription(string description);
    public IEventConfiguration<TEvent> AddRule(Action<IRuleConfiguration<TEvent>> configure);
    public IEventConfiguration<TEvent> AddRule<U>(Action<IRuleConfiguration<TEvent, U>> configure);

    public IEventConfiguration<TEvent> AddAction<TEventOut>(
        Action<IActionConfiguration<TEvent, TEventOut>> configure)
        where TEventOut : class, IEvent;
}
