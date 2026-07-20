namespace RpgCompanion.Host.Configuration;

using Toolbox;

internal class PluginConfiguration(
    IServiceCollection _services,
    PluginMetadata _metadata)
    : IPluginConfiguration
{
    private PluginKey _key = Guid.CreateVersion7().ToString();

    private readonly Dictionary<Type, Action> _intentRegistrations = new();

    private string? _name;
    private string? _version;

    public PluginDescriptor Build()
    {
        _intentRegistrations.Values.ForEach(r => r());
        var descriptor = new PluginDescriptor
        {
            Key = _key,
            Name = _name,
            Version = _version,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return descriptor;
    }

    public IPluginConfiguration WithKey(PluginKey key) => Do(() => _key = key);

    public IPluginConfiguration WithName(string name) => Do(() => _name = name);

    public IPluginConfiguration WithVersion(string version) => Do(() => _version = version);

    public IPluginConfiguration WithInitialization(InitializationHandler handler) =>
        Do(() => _metadata.Initialization = new InitializationExecutor.Sync(handler));

    public IPluginConfiguration WithInitialization(InitializationHandlerAsync handler) =>
        Do(() => _metadata.Initialization = new InitializationExecutor.Async(handler));

    public IPluginConfiguration AddIntent<TIntent>(IntentHandler<TIntent> handler) where TIntent : IIntent =>
        Do(() => _intentRegistrations[typeof(TIntent)] = () =>
            _services.AddKeyedSingleton<IntentExecutor>(typeof(TIntent), new IntentExecutor.Sync<TIntent>(handler)));

    public IPluginConfiguration AddIntent<TIntent>(IntentHandlerAsync<TIntent> handler) where TIntent : IIntent =>
        Do(() => _intentRegistrations[typeof(TIntent)] = () =>
            _services.AddKeyedSingleton<IntentExecutor>(typeof(TIntent), new IntentExecutor.Async<TIntent>(handler)));

    public IPluginConfiguration AddIntent<TIntent, TResult>(IntentHandler<TIntent, TResult> handler) where TIntent : IIntent<TResult> =>
        Do(() => _intentRegistrations[typeof(TIntent)] = () =>
            _services.AddKeyedSingleton<IntentExecutor>(typeof(TIntent), new IntentExecutor.SyncResult<TIntent, TResult>(handler)));

    public IPluginConfiguration AddIntent<TIntent, TResult>(IntentHandlerAsync<TIntent, TResult> handler) where TIntent : IIntent<TResult> =>
        Do(() => _intentRegistrations[typeof(TIntent)] = () =>
            _services.AddKeyedSingleton<IntentExecutor>(typeof(TIntent), new IntentExecutor.AsyncResult<TIntent, TResult>(handler)));

    private IPluginConfiguration Do(Action action)
    {
        action();
        return this;
    }
}
