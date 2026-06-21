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
            Plugin = _plugin,
            Event = _event,
            Actor = _actor,
            Conditions = _conditions,
            ForEvent = null,
            ForRule = null,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IRuleConfiguration<T, U> WithKey(RuleKey<T, U> key) => Do(() => _key = key);
    public IRuleConfiguration<T, U> WithName(string name) => Do(() => _displayName = name);
    public IRuleConfiguration<T, U> WithDescription(string description) => Do(() => _description = description);
    public IRuleConfiguration<T, U> WithOrder(double order) => Do(() => _order = order);

    public IRuleConfiguration<T, U> WithCondition(Action<IConditionConfiguration<T>> configure) => DoLazy(() =>
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

    public IRuleConfiguration<T, U> Export(Rule<T, U> rule) => DoLazy(() =>
    {
        _services.AddKeyedSingleton(_key, rule);
        _services.AddSingleton(rule);
    });

    private IRuleConfiguration<T, U> Do(Action action)
    {
        action();
        return this;
    }

    private IRuleConfiguration<T, U> DoLazy(Action action)
    {
        _lazyConfigurations.Add(action);
        return this;
    }
}
