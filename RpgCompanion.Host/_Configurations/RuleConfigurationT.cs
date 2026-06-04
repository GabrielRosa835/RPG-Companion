namespace RpgCompanion.Host;

using Core;

internal class RuleConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules,
    EventKey? _event,
    ActorKey? _actor)
    : IRuleConfiguration<T>
{
    private readonly List<System.Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _conditions = [];
    private RuleKey _key = Guid.NewGuid().ToString();
    private string? _displayName;
    private string? _description;
    private double? _order;

    internal RuleKey Build()
    {
        foreach (System.Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new RuleDescriptor
        {
            Key = _key,
            Order = _order ?? 0,
            DisplayName = _displayName,
            Description = _description,
            Connections = new()
            {
                Plugin = _plugin,
                Conditions = _conditions,
                Event = _event,
                Actor = _actor,
                ForRule = null,
            }
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IRuleConfiguration<T> WithKey(RuleKey<T> key)
    {
        _key = key;
        return this;
    }

    public IRuleConfiguration<T> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IRuleConfiguration<T> WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public IRuleConfiguration<T> WithOrder(double order)
    {
        _order = order;
        return this;
    }

    public IRuleConfiguration<T> WithCondition(Configure<IConditionConfiguration<T>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ConditionConfiguration<T>(
                _services: _services,
                _plugin: _plugin,
                _for: _key);
            configure(configuration);
            RuleKey key = configuration.Build();
            _conditions.Add(key);
            _pluginRules.Add(key);
        });
        return this;
    }

    public IRuleConfiguration<T> Export(IRule<T> instance)
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedSingleton(_key, instance);
            _services.AddSingleton(instance);
        });
        return this;
    }

    public IRuleConfiguration<T> Export<TRule>() where TRule : class, IRule<T>
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedTransient<IRule<T>>(_key, (sp, key) => sp.GetRequiredKeyedService<TRule>(key));
            _services.AddTransient<IRule<T>>(sp => sp.GetRequiredService<TRule>());
            _services.AddKeyedTransient<TRule>(_key);
            _services.AddTransient<TRule>();
        });
        return this;
    }
}
