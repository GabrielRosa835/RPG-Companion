namespace RpgCompanion.Host;

using Core;

public class ActorConfiguration<TActor>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules,
    ISet<EffectKey> _pluginEffects)
    : IActorConfiguration<TActor> where TActor : class
{
    private readonly ActorKey _key = new();
    private readonly HashSet<RuleKey> _rules = [];
    private readonly HashSet<EffectKey> _effects = [];
    private string? _displayName;

    public ActorDescriptor Build()
    {
        var descriptor = new ActorDescriptor
        {
            Key = _key,
            Plugin = _plugin,
            DisplayName = _displayName,
            Effects = _effects,
            Rules = _rules,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public IActorConfiguration<TActor> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IActorConfiguration<TActor> Export(TActor instance)
    {
        _services.AddKeyedSingleton(_key, instance);
        return this;
    }

    public IActorConfiguration<TActor> Export(Func<IRegistry, ActorKey, TActor> factory)
    {
        _services.AddKeyedSingleton(_key, (sp, key) => factory(sp.AsRegistry(), (ActorKey) key));
        return this;
    }

    public IActorConfiguration<TActor> AddRule(Action<IRuleConfiguration<TActor>> configure)
    {
        var configuration = new RuleConfiguration<TActor>(_services, _plugin, null, _key);
        configure(configuration);
        var descriptor = configuration.Build();
        _pluginRules.Add(descriptor.Key);
        _rules.Add(descriptor.Key);
        return this;
    }

    public IActorConfiguration<TActor> AddEffect(Action<IEffectConfiguration<TActor>> configure)
    {
        var configuration = new EffectConfiguration<TActor>(_services, _plugin, null, _key);
        configure(configuration);
        var descriptor = configuration.Build();
        _pluginEffects.Add(descriptor.Key);
        _effects.Add(descriptor.Key);
        return this;
    }
}
