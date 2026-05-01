namespace RpgCompanion.Core;

public interface IActorConfiguration<TActor> where TActor : class, IActor
{
    public IActorConfiguration<TActor> WithKey(ActorKey<TActor> key);
    public IActorConfiguration<TActor> WithLifetime(Lifetime lifetime);
    public IActorConfiguration<TActor> WithName(string name);

    public IActorConfiguration<TActor> AddRule(Configure<IRuleConfiguration<TActor>> configure);
    public IActorConfiguration<TActor> AddRule<U>(Configure<IRuleConfiguration<TActor, U>> configure);

    public IActorConfiguration<TActor> Export();
}
