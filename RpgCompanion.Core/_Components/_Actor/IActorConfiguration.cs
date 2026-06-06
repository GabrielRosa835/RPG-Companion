namespace RpgCompanion.Core;

public interface IActorConfiguration<TActor> where TActor : class, IActor
{
    public IActorConfiguration<TActor> WithKey(ActorKey<TActor> key);
    public IActorConfiguration<TActor> WithLifetime(ActorLifetime actorLifetime);
    public IActorConfiguration<TActor> WithName(string name);
    public IActorConfiguration<TActor> WithDescription(string description);

    public IActorConfiguration<TActor> AddRule(Action<IRuleConfiguration<TActor>> configure);
    public IActorConfiguration<TActor> AddRule<U>(Action<IRuleConfiguration<TActor, U>> configure);

    public IActorConfiguration<TActor> AddAction<TEvent>(Action<IActionConfiguration<TActor, TEvent>> configure)
        where TEvent : class, IEvent;

    public IActorConfiguration<TActor> Export();
}
