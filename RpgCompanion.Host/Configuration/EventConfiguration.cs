namespace RpgCompanion.Host.Configuration;

using Core;
using Core.Events;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class EventConfiguration<TEvent>(IServiceCollection _services, EventKey _key) where TEvent : IEvent
{
    private readonly HashSet<RuleKey> _rules = [];
    private EffectKey? _effect;
    private string? _displayName;
    private int? _priority;

    internal EventDescriptor Build()
    {
        var descriptor = new EventDescriptor
        {
            Key = _key,
            Effect = _effect!.Value,
            Rules = _rules,
            DisplayName = _displayName,
            Priority = _priority ?? 0,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public EventConfiguration<TEvent> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public EventConfiguration<TEvent> WithPriority(int priority)
    {
        _priority = priority;
        return this;
    }

    public EventConfiguration<TEvent> AddRule(string key, Action<RuleConfiguration<TEvent>> configure)
        => AddRuleInternal(new(key), configure);

    public EventConfiguration<TEvent> AddRule(Action<RuleConfiguration<TEvent>> configure)
        => AddRuleInternal(new(), configure);

    private EventConfiguration<TEvent> AddRuleInternal(RuleKey ruleKey, Action<RuleConfiguration<TEvent>> configure)
    {
        var builder = new RuleConfiguration<TEvent>(_services, ruleKey, _key);
        configure(builder);
        var rule = builder.Build();
        _rules.Add(rule.Key);
        return this;
    }

    public EventConfiguration<TEvent> AddEffect(string key, Action<EffectConfiguration<TEvent>> configure)
        => AddEffectInternal(new(key), configure);

    public EventConfiguration<TEvent> AddEffect(Action<EffectConfiguration<TEvent>> configure)
        => AddEffectInternal(new(), configure);

    public EventConfiguration<TEvent> AddEffectInternal(EffectKey effectKey, Action<EffectConfiguration<TEvent>> configure)
    {
        var builder = new EffectConfiguration<TEvent>(_services, effectKey, _key);
        configure(builder);
        var effect = builder.Build();
        _effect = effect.Key;
        return this;
    }
}
