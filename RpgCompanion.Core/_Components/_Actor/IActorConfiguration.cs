namespace RpgCompanion.Core;

public interface IActorConfiguration<TActor> where TActor : class, IActor
{
    public IActorConfiguration<TActor> WithKey(ActorKey<TActor> key);
    public IActorConfiguration<TActor> WithLifetime(ActorLifetime actorLifetime);
    public IActorConfiguration<TActor> WithName(string name);
    public IActorConfiguration<TActor> WithDescription(string description);

    public IActorConfiguration<TActor> AddRule(Configure<IRuleConfiguration<TActor>> configure);
    public IActorConfiguration<TActor> AddRule<U>(Configure<IRuleConfiguration<TActor, U>> configure);

    public IActorConfiguration<TActor> AddAction<TEvent>(Configure<IActionConfiguration<TActor, TEvent>> configure)
        where TEvent : class, IEvent;

    public IActorConfiguration<TActor> Export();
}
