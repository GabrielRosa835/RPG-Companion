namespace RpgCompanion.Core;

public interface IActorConfiguration<TActor> where TActor : class
{
    public IActorConfiguration<TActor> WithName(string name);
    public IActorConfiguration<TActor> Export(TActor instance);
    public IActorConfiguration<TActor> Export(Func<IRegistry, ActorKey, TActor> factory);

    public IActorConfiguration<TActor> AddRule(Action<IRuleConfiguration<TActor>> configure);
    public IActorConfiguration<TActor> AddEffect(Action<IEffectConfiguration<TActor>> configure);
}
