namespace RpgCompanion.Host;

using Core;

internal class ActionConfiguration<T, TEvent>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules,
    ActorKey? _actor,
    EventKey? _event)
    : IActionConfiguration<T, TEvent>
    where TEvent : class, IEvent
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _conditions = [];
    private RuleKey? _key;
    private EventKey? _for;
    private string? _displayName;
    private string? _description;

    internal RuleKey Build()
    {
        KeyException.ThrowIfNull(_key);
        KeyException.ThrowIfNull(_for);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new RuleDescriptor
        {
            Key = _key.Value,
            Order = 0,
            DisplayName = _displayName,
            Description = _description,
            Connections = new()
            {
                Event = _event,
                Actor = _actor,
                ForEvent = _for,
                ForRule = null,
                Plugin = _plugin,
                Conditions = _conditions,
            }
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key.Value;
    }

    public IActionConfiguration<T, TEvent> WithKey(RuleKey<T, TEvent> key)
    {
        _key = key;
        return this;
    }

    public IActionConfiguration<T, TEvent> ForEvent(EventKey<TEvent> key)
    {
        _for = key;
        return this;
    }

    public IActionConfiguration<T, TEvent> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IActionConfiguration<T, TEvent> WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public IActionConfiguration<T, TEvent> WithCondition(Configure<IConditionConfiguration<T>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ConditionConfiguration<T>(
                _services: _services,
                _plugin: _plugin,
                _for: _key!.Value);
            configure(configuration);
            var key = configuration.Build();
            _conditions.Add(key);
            _pluginRules.Add(key);
        });
        return this;
    }

    public IActionConfiguration<T, TEvent> Export(Rule<T, TEvent> instance)
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedSingleton(_key, instance);
            _services.AddKeyedSingleton<Rule<T, IEvent>>(_key, instance);
            _services.AddSingleton(instance);
            _services.AddSingleton<Rule<T, IEvent>>(instance);
        });
        return this;
    }

    public IActionConfiguration<T, TEvent> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T, TEvent>
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedTransient<TDefinition>(_key);
            _services.AddKeyedTransient<Rule<T, TEvent>>(_key, (sp, key) =>
                sp.GetRequiredKeyedService<TDefinition>(_key).Compose());
            _services.AddKeyedTransient<Rule<T, IEvent>>(_key, (sp, key) =>
                sp.GetRequiredKeyedService<TDefinition>(_key).Compose());

            _services.AddTransient<TDefinition>();
            _services.AddTransient<Rule<T, TEvent>>(sp =>
                sp.GetRequiredService<TDefinition>().Compose());
            _services.AddTransient<Rule<T, IEvent>>(sp =>
                sp.GetRequiredService<TDefinition>().Compose());
        });
        return this;
    }
}
