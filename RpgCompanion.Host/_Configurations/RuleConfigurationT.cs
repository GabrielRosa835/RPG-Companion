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
            Order = _order ?? 0,
            DisplayName = _displayName,
            Description = _description,
            Plugin = _plugin,
            Conditions = _conditions,
            Event = _event,
            Actor = _actor,
            ForRule = null,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IRuleConfiguration<T> WithKey(RuleKey<T> key) => Do(() => _key = key);
    public IRuleConfiguration<T> WithName(string name) => Do(() => _displayName = name);
    public IRuleConfiguration<T> WithDescription(string description) => Do(() => _description = description);
    public IRuleConfiguration<T> WithOrder(double order) => Do(() => _order = order);

    public IRuleConfiguration<T> WithCondition(Action<IConditionConfiguration<T>> configure) => DoLazy(() =>
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

    public IRuleConfiguration<T> Export(Rule<T> rule) => DoLazy(() =>
    {
        _services.AddKeyedSingleton(_key, rule);
        _services.AddSingleton(rule);
    });

    private IRuleConfiguration<T> Do(Action action)
    {
        action();
        return this;
    }

    private IRuleConfiguration<T> DoLazy(Action action)
    {
        _lazyConfigurations.Add(action);
        return this;
    }
}
