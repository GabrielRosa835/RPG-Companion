namespace RpgCompanion.Host.Configuration;

using Core;
using Microsoft.Extensions.DependencyInjection.Extensions;

internal class PluginConfiguration(
    IServiceCollection _services,
    PluginMetadata _metadata)
    : IPluginConfiguration
{
    private readonly List<Action> _lazyConfigurations = [];
    private PluginKey? _key;

    private readonly HashSet<RuleKey> _rules = [];
    private readonly HashSet<EventKey> _events = [];
    private readonly HashSet<ActorKey> _actors = [];

    private string? _name;
    private string? _version;

    public PluginDescriptor Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new PluginDescriptor
        {
            Key = _key.Value,
            Name = _name,
            Version = _version,
            Events = _events,
            Rules = _rules,
            Actors = _actors,
        };
        _services.TryAddKeyedSingleton(_key, descriptor);
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

    public IPluginConfiguration AddActor<TActor>(Configure<IActorConfiguration<TActor>> configure)
        where TActor : class, IActor
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ActorConfiguration<TActor>(
                _services: _services,
                _plugin: _key!.Value,
                _pluginRules: _rules);
            configure(configuration);
            ActorKey key = configuration.Build();
            _actors.Add(key);
        });
        return this;
    }

    public IPluginConfiguration AddEvent<TEvent>(Configure<IEventConfiguration<TEvent>> configure)
        where TEvent : class, IEvent
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new EventConfiguration<TEvent>(
                _services: _services,
                _plugin: _key!.Value,
                _pluginRules: _rules);
            configure(configuration);
            EventKey key = configuration.Build();
            _events.Add(key);
        });
        return this;
    }

    public IPluginConfiguration AddRule<T>(Configure<IRuleConfiguration<T>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<T>(
                _services: _services,
                _plugin: _key!.Value,
                _pluginRules: _rules,
                _event: null,
                _actor: null);
            configure(configuration);
            RuleKey key = configuration.Build();
            _rules.Add(key);
        });
        return this;
    }

    public IPluginConfiguration AddRule<T, U>(Configure<IRuleConfiguration<T, U>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<T, U>(
                _services: _services,
                _plugin: _key!.Value,
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
