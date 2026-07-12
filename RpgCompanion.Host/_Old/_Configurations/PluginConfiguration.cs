namespace RpgCompanion.Host.Configuration;

using Events;

internal class PluginConfiguration(
    IServiceCollection _services,
    PluginMetadata _metadata)
    : IPluginConfiguration
{
    private readonly List<Action> _lazyConfigurations = [];
    private PluginKey _key = Guid.CreateVersion7().ToString();

    private readonly HashSet<RuleKey> _rules = [];
    private readonly HashSet<EventKey> _events = [];
    private readonly HashSet<ActorKey> _actors = [];

    private string? _name;
    private string? _version;

    public PluginDescriptor Build()
    {
        foreach (var lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new PluginDescriptor
        {
            Key = _key,
            Name = _name,
            Version = _version,
            Events = _events,
            Rules = _rules,
            Actors = _actors,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        _services.AddKeyedSingleton<World>(_key);
        return descriptor;
    }

    public IPluginConfiguration WithKey(PluginKey key) => Do(() => _key = key);

    public IPluginConfiguration WithName(string name) => Do(() => _name = name);

    public IPluginConfiguration WithVersion(string version) => Do(() => _version = version);

    public IPluginConfiguration WithInitialization(Initialization initialization) =>
        Do(() => _metadata.Initialization = initialization);

    public IPluginConfiguration AddActor<TActor>(Action<IActorConfiguration<TActor>> configure)
        where TActor : class, IActor => DoLazy(() =>
    {
        var configuration = new ActorConfiguration<TActor>(
            _services: _services,
            _plugin: _key,
            _pluginRules: _rules);
        configure(configuration);
        ActorKey key = configuration.Build();
        _actors.Add(key);
    });

    public IPluginConfiguration AddEvent<TEvent>(Action<IEventConfiguration<TEvent>> configure)
        where TEvent : class, IEvent => DoLazy(() =>
    {
        var configuration = new EventConfiguration<TEvent>(
            _services: _services,
            _plugin: _key,
            _pluginRules: _rules);
        configure(configuration);
        EventKey key = configuration.Build();
        _events.Add(key);
    });

    public IPluginConfiguration AddRule<T>(Action<IRuleConfiguration<T>> configure) => DoLazy(() =>
    {
        var configuration = new RuleConfiguration<T>(
            _services: _services,
            _plugin: _key,
            _pluginRules: _rules,
            _event: null,
            _actor: null);
        configure(configuration);
        RuleKey key = configuration.Build();
        _rules.Add(key);
    });

    public IPluginConfiguration AddRule<T, U>(Action<IRuleConfiguration<T, U>> configure) => DoLazy(() =>
    {
        var configuration = new RuleConfiguration<T, U>(
            _services: _services,
            _plugin: _key,
            _pluginRules: _rules,
            _event: null,
            _actor: null);
        configure(configuration);
        RuleKey key = configuration.Build();
        _rules.Add(key);
    });

    private IPluginConfiguration Do(Action action)
    {
        action();
        return this;
    }

    private IPluginConfiguration DoLazy(Action action)
    {
        _lazyConfigurations.Add(action);
        return this;
    }
}
