namespace RpgCompanion.Host;

using Core;

internal class RuleConfiguration<T, U>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules,
    EventKey? _event,
    ActorKey? _actor)
    : IRuleConfiguration<T, U>
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _conditions = [];
    private RuleKey _key = Guid.NewGuid().ToString();
    private string? _displayName;
    private string? _description;
    private double? _order;

    internal RuleKey Build()
    {
        foreach (var lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new RuleDescriptor
        {
            Key = _key,
            DisplayName = _displayName,
            Description = _description,
            Order = _order ?? 0,
            Connections = new()
            {
                Plugin = _plugin,
                Event = _event,
                Actor = _actor,
                Conditions =  _conditions,
                ForEvent = null,
                ForRule = null,
            }
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IRuleConfiguration<T, U> WithKey(RuleKey<T, U> key)
    {
        _key = key;
        return this;
    }

    public IRuleConfiguration<T, U> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IRuleConfiguration<T, U> WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public IRuleConfiguration<T, U> WithOrder(double order)
    {
        _order = order;
        return this;
    }

    public IRuleConfiguration<T, U> WithCondition(Action<IConditionConfiguration<T>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ConditionConfiguration<T>(
                _services: _services,
                _plugin: _plugin,
                _for: _key);
            configure(configuration);
            var key = configuration.Build();
            _conditions.Add(key);
            _pluginRules.Add(key);
        });
        return this;
    }

    public IRuleConfiguration<T, U> Export(IRule<T, U> instance)
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedSingleton(_key, instance);
            _services.AddSingleton(instance);
        });
        return this;
    }

    public IRuleConfiguration<T, U> Export<TRule>() where TRule : class, IRule<T, U>
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedTransient<IRule<T, U>>(_key, (sp, key) => sp.GetRequiredKeyedService<TRule>(key));
            _services.AddTransient<IRule<T, U>>(sp => sp.GetRequiredService<TRule>());
            _services.AddKeyedTransient<TRule>(_key);
            _services.AddTransient<TRule>();
        });
        return this;
    }
}
