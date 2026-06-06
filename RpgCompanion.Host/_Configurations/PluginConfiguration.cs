namespace RpgCompanion.Host.Configuration;

using Core;

internal class PluginConfiguration(
    IServiceCollection _services,
    PluginMetadata _metadata)
    : IPluginConfiguration
{
    private readonly List<Action> _lazyConfigurations = [];
    private PluginKey _key = Guid.NewGuid().ToString();

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
        return descriptor;
    }

    public IPluginConfiguration WithKey(PluginKey key)
    {
        _key = key;
        return this;
    }

    public IPluginConfiguration WithName(string name)
    {
        _name = name;
        return this;
    }

    public IPluginConfiguration WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public IPluginConfiguration WithInitialization(Initialization initialization)
    {
        _metadata.Initialization = initialization;
        return this;
    }

    public IPluginConfiguration AddActor<TActor>(Action<IActorConfiguration<TActor>> configure)
        where TActor : class, IActor
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ActorConfiguration<TActor>(
                _services: _services,
                _plugin: _key,
                _pluginRules: _rules);
            configure(configuration);
            ActorKey key = configuration.Build();
            _actors.Add(key);
        });
        return this;
    }

    public IPluginConfiguration AddEvent<TEvent>(Action<IEventConfiguration<TEvent>> configure)
        where TEvent : class, IEvent
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new EventConfiguration<TEvent>(
                _services: _services,
                _plugin: _key,
                _pluginRules: _rules);
            configure(configuration);
            EventKey key = configuration.Build();
            _events.Add(key);
        });
        return this;
    }

    public IPluginConfiguration AddRule<T>(Action<IRuleConfiguration<T>> configure)
    {
        _lazyConfigurations.Add(() =>
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
        return this;
    }

    public IPluginConfiguration AddRule<T, U>(Action<IRuleConfiguration<T, U>> configure)
    {
        _lazyConfigurations.Add(() =>
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
        return this;
    }
}
