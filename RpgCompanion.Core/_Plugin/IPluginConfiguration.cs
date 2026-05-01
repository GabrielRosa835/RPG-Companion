namespace RpgCompanion.Core;

public interface IPluginConfiguration
{
    public IPluginConfiguration WithKey(PluginKey key);
    public IPluginConfiguration WithName(string name);
    public IPluginConfiguration WithVersion(string version);
    public IPluginConfiguration WithInitialization(Initialization initialization);

    public IPluginConfiguration AddActor<TActor>(Configure<IActorConfiguration<TActor>> configure)
        where TActor : class, IActor;

    public IPluginConfiguration AddEvent<TEvent>(Configure<IEventConfiguration<TEvent>> configure)
        where TEvent : IEvent;

    public IPluginConfiguration AddRule<T>(Configure<IRuleConfiguration<T>> configure);
    public IPluginConfiguration AddRule<T, U>(Configure<IRuleConfiguration<T, U>> configure);
}
