namespace RpgCompanion.Host;

using Core;

public class ConditionConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    RuleKey _for)
    : IConditionConfiguration<T>
{
    private readonly List<Action> _lazyConfigurations = [];
    private RuleKey? _key;
    private string? _displayName;
    private string? _description;

    public RuleKey Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new RuleDescriptor
        {
            Key = _key.Value,
            Order = 0,
            Description =  _description,
            DisplayName = _displayName,
            Connections = new ()
            {
                Event = null,
                Actor = null,
                Conditions = null,
                ForRule = _for,
                Plugin = _plugin,
            }
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key.Value;
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

    public IConditionConfiguration<T> Export(Rule<T, bool> instance)
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedSingleton(_key, instance);
            _services.AddSingleton(instance);
        });
        return this;
    }

    public IConditionConfiguration<T> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T, bool>
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedTransient<TDefinition>(_key);
            _services.AddTransient<TDefinition>();
            _services.AddKeyedTransient<Rule<T, bool>>(_key,
                (sp, key) => sp.GetRequiredService<TDefinition>().Compose());
            _services.AddTransient<Rule<T, bool>>(sp => sp.GetRequiredService<TDefinition>().Compose());
        });
        return this;
    }
}
