namespace RpgCompanion.Host;

using Events;

internal class EventConfiguration<TEvent>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules)
    : IEventConfiguration<TEvent>
    where TEvent : class, IEvent
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _actions = [];
    private readonly HashSet<RuleKey> _rules = [];
    private EventKey _key = Guid.NewGuid().ToString();
    private string? _displayName;
    private string? _description;

    internal EventKey Build()
    {
        foreach (var lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new EventDescriptor
        {
            Key = _key,
            DisplayName = _displayName,
            Description = _description,
            Type = typeof(TEvent),
            Plugin = _plugin,
            Actions = _actions,
            Rules = _rules,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IEventConfiguration<TEvent> WithKey(EventKey<TEvent> key)
    {
        _key = key;
        return this;
    }

    public IEventConfiguration<TEvent> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IEventConfiguration<TEvent> WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public IEventConfiguration<TEvent> AddRule(Action<IRuleConfiguration<TEvent>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TEvent>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: _key,
                _actor: null);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }

    public IEventConfiguration<TEvent> AddRule<U>(Action<IRuleConfiguration<TEvent, U>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TEvent, U>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: _key,
                _actor: null);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }

    public IEventConfiguration<TEvent> AddAction<TOtherEvent>(
        Action<IActionConfiguration<TEvent, TOtherEvent>> configure) where TOtherEvent : class, IEvent
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ActionConfiguration<TEvent, TOtherEvent>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _actor: null,
                _event: _key);
            configure(configuration);
            var key = configuration.Build();
            _actions.Add(key);
            _pluginRules.Add(key);
        });
        return this;
    }
}
