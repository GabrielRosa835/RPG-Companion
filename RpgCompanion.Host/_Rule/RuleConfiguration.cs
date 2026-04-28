namespace RpgCompanion.Host;

using Core;

internal class RuleConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    EventKey? _event,
    ActorKey? _actor)
    : IRuleConfiguration<T>
{
    private readonly RuleKey _key = new();
    private ConditionKey? _condition;
    private string? _displayName;

    internal RuleDescriptor Build()
    {
        var descriptor = new RuleDescriptor
        {
            Key = _key,
            Event = _event,
            Actor = _actor,
            Condition = _condition,
            DisplayName = _displayName,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public IRuleConfiguration<T> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IRuleConfiguration<T> WithCondition(Action<IConditionConfiguration<T>> configure)
    {
        var configuration = new ConditionConfiguration<T>(_services, _plugin, null, _key);
        configure(configuration);
        var descriptor = configuration.Build();
        _condition = descriptor.Key;
        return this;
    }

    public IRuleConfiguration<T> Export(Rule<T> instance)
    {
        _services.AddKeyedSingleton(_key, instance);
        return this;
    }

    public IRuleConfiguration<T> Export(Func<IRegistry, RuleKey, Rule<T>> factory)
    {
        _services.AddKeyedSingleton(_key, (sp, key) => factory(sp.AsRegistry(), (RuleKey) key));
        return this;
    }
}
