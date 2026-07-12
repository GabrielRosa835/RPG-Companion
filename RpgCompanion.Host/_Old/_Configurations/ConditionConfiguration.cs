namespace RpgCompanion.Host;

using Events;

public class ConditionConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    RuleKey _for)
    : IConditionConfiguration<T>
{
    private readonly List<Action> _lazyConfigurations = [];
    private RuleKey _key = Guid.CreateVersion7().ToString();
    private string? _displayName;
    private string? _description;

    public RuleKey Build()
    {
        foreach (var lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new RuleDescriptor
        {
            Key = _key,
            Order = 0,
            Description = _description,
            DisplayName = _displayName,
            Event = null,
            Actor = null,
            Conditions = new HashSet<RuleKey>(),
            ForRule = _for,
            Plugin = _plugin,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IConditionConfiguration<T> WithKey(RuleKey<T, bool> key) => Do(() => _key = key);
    public IConditionConfiguration<T> WithName(string name) => Do(() => _displayName = name);
    public IConditionConfiguration<T> WithDescription(string description) => Do(() => _description = description);

    public IConditionConfiguration<T> Export(Rule<T, bool> instance) => DoLazy(() =>
    {
        _services.AddKeyedSingleton(_key, instance);
        _services.AddSingleton(instance);
    });

    private IConditionConfiguration<T> Do(Action action)
    {
        action();
        return this;
    }

    private IConditionConfiguration<T> DoLazy(Action action)
    {
        _lazyConfigurations.Add(action);
        return this;
    }
}
