namespace RpgCompanion.Host;

using Core;
using Action = System.Action;

public class ConditionConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    RuleKey _for)
    : IConditionConfiguration<T>
{
    private readonly List<Action> _lazyConfigurations = [];
    private RuleKey _key = Guid.NewGuid().ToString();
    private string? _displayName;
    private string? _description;

    public RuleKey Build()
    {
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new RuleDescriptor
        {
            Key = _key,
            Order = 0,
            Description =  _description,
            DisplayName = _displayName,
            Connections = new ()
            {
                Event = null,
                Actor = null,
                Conditions = new HashSet<RuleKey>(),
                ForRule = _for,
                Plugin = _plugin,
            }
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key;
    }

    public IConditionConfiguration<T> WithKey(RuleKey<T, bool> key)
    {
        _key = key;
        return this;
    }

    public IConditionConfiguration<T> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IConditionConfiguration<T> WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public IConditionConfiguration<T> Export(IRule<T, bool> instance)
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedSingleton(_key, instance);
            _services.AddSingleton(instance);
        });
        return this;
    }

    public IConditionConfiguration<T> Export<TRule>() where TRule : class, IRule<T, bool>
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedTransient<TRule>(_key);
            _services.AddTransient<TRule>();
            _services.AddKeyedTransient<IRule<T, bool>>(_key, (sp, key) => sp.GetRequiredKeyedService<TRule>(key));
            _services.AddTransient<IRule<T, bool>>(sp => sp.GetRequiredService<TRule>());
        });
        return this;
    }
}
