namespace RpgCompanion.QuartzPrototype.Configuration;

using Descriptors;
using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

internal class EventBuilder<TEvent>(IServiceCollection services, string? key) where TEvent : IEvent
{
    private EventDescriptor _descriptor = new(key ?? Guid.NewGuid().ToString());

    internal EventDescriptor Build()
    {
        services.AddKeyedSingleton(_descriptor.Key, _descriptor);
        return _descriptor;
    }

    public EventBuilder<TEvent> WithName(string name)
    {
        _descriptor.DisplayName = name;
        return this;
    }

    public EventBuilder<TEvent> WithPriority(int priority)
    {
        _descriptor.Priority = priority;
        return this;
    }

    public EventBuilder<TEvent> AddRule(string key, Action<RuleBuilder<TEvent>> configure)
        => AddRuleInternal(key, configure);

    public EventBuilder<TEvent> AddRule(Action<RuleBuilder<TEvent>> configure)
        => AddRuleInternal(null, configure);

    private EventBuilder<TEvent> AddRuleInternal(string? key, Action<RuleBuilder<TEvent>> configure)
    {
        var builder = new RuleBuilder<TEvent>(services, key);
        configure(builder);
        var rule = builder.Build();
        _descriptor.Rules.Add(rule.Key);
        return this;
    }

    public EventBuilder<TEvent> AddEffect(string key, Action<EffectBuilder<TEvent>> configure)
        => AddEffectInternal(key, configure);

    public EventBuilder<TEvent> AddEffect(Action<EffectBuilder<TEvent>> configure)
        => AddEffectInternal(null, configure);

    public EventBuilder<TEvent> AddEffectInternal(string? key, Action<EffectBuilder<TEvent>> configure)
    {
        var builder = new EffectBuilder<TEvent>(services, key);
        configure(builder);
        var effect = builder.Build();
        _descriptor.Effect = effect.Key;
        return this;
    }
}
