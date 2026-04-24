namespace RpgCompanion.Core;

public interface IPluginConfiguration
{
    public IPluginConfiguration AddEvent<TEvent>(Action<IEventConfiguration<TEvent>> configure) where TEvent : IEvent;
    public IPluginConfiguration WithName(string name);
    public IPluginConfiguration WithVersion(string version);
    public IPluginConfiguration WithInitialization(Action<IInitializationConfiguration> configure);
}
