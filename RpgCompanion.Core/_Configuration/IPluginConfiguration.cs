namespace RpgCompanion.Core;

public interface IPluginConfiguration
{
    void WithKey(PluginKey key);
    void WithIdentifier(string identifier);
    void WithName(string name);
    void WithVersion(string version);
    void WithInitialization<TInitialization>() where TInitialization : class, IInitialization;
    void WithAsyncInitialization<TInitialization>() where TInitialization : class, IAsyncInitialization;
    void AddIntent<TIntent>(Action<IIntentConfiguration<TIntent>> configure) where TIntent : IIntent;
    void AddIntent<TIntent, TResult>(Action<IIntentConfiguration<TIntent, TResult>> configure) where TIntent : IIntent<TResult>;
    void AddEvent<TEvent>(Action<IEventConfiguration<TEvent>> configure) where TEvent : Event;
    void AddEntity<TEntity>(Action<IEntityConfiguration<TEntity>> configure) where TEntity : IEntity;
}
