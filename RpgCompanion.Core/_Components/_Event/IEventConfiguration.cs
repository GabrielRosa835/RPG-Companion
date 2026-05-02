namespace RpgCompanion.Core;

public interface IEventConfiguration<TEvent> where TEvent : class, IEvent
{
    public IEventConfiguration<TEvent> WithKey(EventKey<TEvent> key);
    public IEventConfiguration<TEvent> WithName(string name);
    public IEventConfiguration<TEvent> WithDescription(string description);
    public IEventConfiguration<TEvent> AddRule(Configure<IRuleConfiguration<TEvent>> configure);
    public IEventConfiguration<TEvent> AddRule<U>(Configure<IRuleConfiguration<TEvent, U>> configure);

    public IEventConfiguration<TEvent> AddAction<TEventOut>(
        Configure<IActionConfiguration<TEvent, TEventOut>> configure)
        where TEventOut : class, IEvent;
}
