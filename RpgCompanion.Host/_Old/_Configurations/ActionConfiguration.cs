namespace RpgCompanion.Host;

using Events;

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
    private RuleKey _key = Guid.CreateVersion7().ToString();
    private EventKey? _for;
    private string? _displayName;
    private string? _description;
    private double? _order;

    internal RuleKey Build()
    {
        KeyException.ThrowIfNull(_for);
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
            Event = _event,
            Actor = _actor,
            ForEvent = _for,
            ForRule = null,
            Plugin = _plugin,
            Conditions = _conditions,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IActionConfiguration<T, TEvent> WithKey(RuleKey<T, TEvent> key) => Do(() => _key = key);
    public IActionConfiguration<T, TEvent> ForEvent(EventKey<TEvent> key) => Do(() => _for = key);
    public IActionConfiguration<T, TEvent> WithName(string name) => Do(() => _displayName = name);
    public IActionConfiguration<T, TEvent> WithDescription(string description) => Do(() => _description = description);
    public IActionConfiguration<T, TEvent> WithOrder(double order) => Do(() => _order = order);

    public IActionConfiguration<T, TEvent> WithCondition(Action<IConditionConfiguration<T>> configure) => DoLazy(() =>
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

    public IActionConfiguration<T, TEvent> Export(Rule<T, TEvent> instance) => DoLazy(() =>
    {
        _services.AddKeyedSingleton(_key, instance);
        _services.AddKeyedSingleton<Rule<T, IEvent>>(_key, instance);
        _services.AddSingleton(instance);
        _services.AddSingleton<Rule<T, IEvent>>(instance);
    });

    private IActionConfiguration<T, TEvent> DoLazy(Action action)
    {
        _lazyConfigurations.Add(action);
        return this;
    }

    private IActionConfiguration<T, TEvent> Do(Action action)
    {
        action();
        return this;
    }
}
