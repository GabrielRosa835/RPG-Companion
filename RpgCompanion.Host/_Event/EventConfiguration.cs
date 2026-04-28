namespace RpgCompanion.Host;

using Core;

internal class EventConfiguration<TEvent>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules,
    ISet<EffectKey> _pluginEffects)
    : IEventConfiguration<TEvent>
    where TEvent : IEvent
{
    private readonly EventKey _key = new();
    private readonly HashSet<EffectKey> _effects = [];
    private readonly HashSet<RuleKey> _rules = [];
    private string? _displayName;
    private int? _priority;

    internal EventDescriptor Build()
    {
        var descriptor = new EventDescriptor
        {
            Key = _key,
            Effects = _effects,
            Rules = _rules,
            DisplayName = _displayName,
            Priority = _priority ?? 0,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public IEventConfiguration<TEvent> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IEventConfiguration<TEvent> WithPriority(int priority)
    {
        _priority = priority;
        return this;
    }

    public IEventConfiguration<TEvent> AddRule(Action<IRuleConfiguration<TEvent>> configure)
    {
        var configuration = new RuleConfiguration<TEvent>(_services, _plugin, _key, null);
        configure(configuration);
        var descriptor = configuration.Build();
        _pluginRules.Add(descriptor.Key);
        _rules.Add(descriptor.Key);
        return this;
    }

    public IEventConfiguration<TEvent> AddEffect(Action<IEffectConfiguration<TEvent>> configure)
    {
        var configuration = new EffectConfiguration<TEvent>(_services, _plugin, _key, null);
        configure(configuration);
        var descriptor = configuration.Build();
        _pluginEffects.Add(descriptor.Key);
        _effects.Add(descriptor.Key);
        return this;
    }
}
