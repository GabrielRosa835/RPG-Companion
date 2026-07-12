namespace RpgCompanion.Host;

using Events;

public class ActorConfiguration<TActor>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules)
    : IActorConfiguration<TActor> where TActor : class, IActor
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _actions = [];
    private readonly HashSet<RuleKey> _rules = [];
    private ActorLifetime _lifetime = ActorLifetime.Immediate;
    private ActorKey _key = Guid.NewGuid().ToString();
    private string? _displayName;
    private string? _description;

    public ActorKey Build()
    {
        foreach (var lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new ActorDescriptor
        {
            Key = _key,
            Lifetime = _lifetime,
            DisplayName = _displayName,
            Description = _description,
            Type = typeof(TActor),
            Plugin = _plugin,
            Rules = _rules,
            Actions = _actions,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IActorConfiguration<TActor> WithKey(ActorKey<TActor> key) => Do(() => _key = key);

    public IActorConfiguration<TActor> WithLifetime(ActorLifetime actorLifetime) => Do(() => _lifetime = actorLifetime);

    public IActorConfiguration<TActor> WithName(string name) => Do(() => _displayName = name);

    public IActorConfiguration<TActor> WithDescription(string description) => Do(() => _description = description);

    public IActorConfiguration<TActor> AddAction<TEvent>(Action<IActionConfiguration<TActor, TEvent>> configure)
        where TEvent : class, IEvent => DoLazy(() =>
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

    public IActorConfiguration<TActor> Export() => DoLazy(() =>
    {
        switch (_lifetime)
        {
            case ActorLifetime.Persistent:
                _services.AddKeyedSingleton<TActor>(_key);
                _services.AddSingleton<TActor>();
                return;
            case ActorLifetime.Temporary:
                _services.AddKeyedScoped<TActor>(_key);
                _services.AddScoped<TActor>();
                return;
            case ActorLifetime.Immediate:
            default:
                _services.AddKeyedTransient<TActor>(_key);
                _services.AddTransient<TActor>();
                break;
        }
    });

    public IActorConfiguration<TActor> AddRule<U>(Action<IRuleConfiguration<TActor, U>> configure) => DoLazy(() =>
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

    public IActorConfiguration<TActor> AddRule(Action<IRuleConfiguration<TActor>> configure) => DoLazy(() =>
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

    private IActorConfiguration<TActor> Do(System.Action action)
    {
        action();
        return this;
    }

    private IActorConfiguration<TActor> DoLazy(System.Action action)
    {
        _lazyConfigurations.Add(action);
        return this;
    }
}
