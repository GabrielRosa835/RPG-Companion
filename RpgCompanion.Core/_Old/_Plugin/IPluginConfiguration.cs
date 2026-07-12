namespace RpgCompanion.Events;

public interface IPluginConfiguration
{
    public IPluginConfiguration WithKey(PluginKey key);
    public IPluginConfiguration WithName(string name);
    public IPluginConfiguration WithVersion(string version);
    public IPluginConfiguration WithInitialization(Initialization initialization);

    public IPluginConfiguration AddActor<TActor>(Action<IActorConfiguration<TActor>> configure)
        where TActor : class, IActor;

    public IPluginConfiguration AddEvent<TEvent>(Action<IEventConfiguration<TEvent>> configure)
        where TEvent : class, IEvent;

    public IPluginConfiguration AddRule<T>(Action<IRuleConfiguration<T>> configure);
    public IPluginConfiguration AddRule<T, U>(Action<IRuleConfiguration<T, U>> configure);

    // // New registration method for synchronous intents
    // public IPluginConfiguration AddIntent<TIntent, THandler, TResult>()
    //     where TIntent : class, IPlayerIntent
    //     where THandler : class, IIntentHandler<TIntent, TResult>;
}
