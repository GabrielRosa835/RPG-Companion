namespace RpgCompanion.Host;

using Core;

internal class EffectConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    EventKey? _event,
    ActorKey? _actor)
    : IEffectConfiguration<T>
{
    private EffectKey _key = new();
    private string? _name;
    private ConditionKey? _condition;

    internal EffectDescriptor Build()
    {
        var descriptor = new EffectDescriptor
        {
            Key = _key,
            Plugin = _plugin,
            Condition = _condition,
            DisplayName = _name,
            Event = _event,
            Actor =  _actor,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public IEffectConfiguration<T> WithName(string name)
    {
        _name = name;
        return this;
    }

    public IEffectConfiguration<T> WithCondition(Action<IConditionConfiguration<T>> configure)
    {
        var configuration = new ConditionConfiguration<T>(_services, _plugin, _key, null);
        configure(configuration);
        var descriptor = configuration.Build();
        _condition = descriptor.Key;
        return this;
    }

    public IEffectConfiguration<T> Export<TResult>(Effect<T, TResult> instance)
    {
        _services.AddKeyedSingleton(_key, instance);
        return this;
    }

    public IEffectConfiguration<T> Export<TResult>(Func<IRegistry, EffectKey, Effect<T, TResult>> factory)
    {
        _services.AddKeyedTransient(_key, (sp, key) => factory(sp.AsRegistry(), (EffectKey) key));
        return this;
    }
}
