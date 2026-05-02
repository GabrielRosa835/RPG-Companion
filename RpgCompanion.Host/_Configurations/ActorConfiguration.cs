namespace RpgCompanion.Host;

using Core;

public class ActorConfiguration<TActor>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules)
    : IActorConfiguration<TActor> where TActor : class, IActor
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _actions = [];
    private readonly HashSet<RuleKey> _rules = [];
    private ActorLifetime? _lifetime;
    private ActorLifetime _registeredLifetime = ActorLifetime.Immediate;
    private ActorKey? _key;
    private string? _displayName;
    private string? _description;

    public ActorKey Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new ActorDescriptor
        {
            Key = _key.Value,
            Lifetime = _registeredLifetime,
            DisplayName = _displayName,
            Description = _description,
            Type = typeof(TActor),
            Connections = new()
            {
                Plugin = _plugin,
                Rules = _rules,
                Actions = _actions,
            }
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key.Value;
    }

    public IActorConfiguration<TActor> WithKey(ActorKey<TActor> key)
    {
        _key = key;
        return this;
    }

    public IActorConfiguration<TActor> WithLifetime(ActorLifetime actorLifetime)
    {
        _lifetime = actorLifetime;
        return this;
    }

    public IActorConfiguration<TActor> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IActorConfiguration<TActor> WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public IActorConfiguration<TActor> AddAction<TEvent>(Configure<IActionConfiguration<TActor, TEvent>> configure)
        where TEvent : class, IEvent
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ActionConfiguration<TActor, TEvent>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: null,
                _actor: _key);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }

    public IActorConfiguration<TActor> Export()
    {
        _lazyConfigurations.Add(() =>
        {
            if (_lifetime == ActorLifetime.Persistent)
            {
                _services.AddKeyedSingleton<TActor>(_key);
                _services.AddSingleton<TActor>();
                _registeredLifetime = ActorLifetime.Persistent;
                return;
            }
            // Defaults to transient
            _services.AddKeyedTransient<TActor>(_key);
            _services.AddTransient<TActor>();
            _registeredLifetime = ActorLifetime.Immediate;
        });
        return this;
    }

    public IActorConfiguration<TActor> AddRule<U>(Configure<IRuleConfiguration<TActor, U>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TActor, U>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: null,
                _actor: _key);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }

    public IActorConfiguration<TActor> AddRule(Configure<IRuleConfiguration<TActor>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TActor>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: null,
                _actor: _key);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }
}
