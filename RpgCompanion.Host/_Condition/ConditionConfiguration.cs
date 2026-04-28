namespace RpgCompanion.Host;

using Core;

public class ConditionConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    EffectKey? _effect,
    RuleKey? _rule)
    : IConditionConfiguration<T>
{
    private readonly ConditionKey _key = new();
    private string? _name;

    public ConditionDescriptor Build()
    {
        var descriptor = new ConditionDescriptor
        {
            Key = _key,
            Plugin = _plugin,
            Effect = _effect,
            Rule = _rule,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public IConditionConfiguration<T> WithName(string name)
    {
        _name = name;
        return this;
    }

    public IConditionConfiguration<T> Export(Condition<T> instance)
    {
        _services.AddKeyedSingleton(_key, instance);
        return this;
    }

    public IConditionConfiguration<T> Export(Func<IRegistry, ConditionKey, Condition<T>> factory)
    {
        _services.AddKeyedTransient<Condition<T>>(_key, (sp, key) => factory(sp.AsRegistry(), (ConditionKey) key));
        return this;
    }
}
