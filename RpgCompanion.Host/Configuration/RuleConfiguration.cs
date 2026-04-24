namespace RpgCompanion.Host.Configuration;

using Core;
using Core.Events;
using Delegates;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class RuleConfiguration<TEvent>(IServiceCollection _services, RuleKey _key, EventKey _event) where TEvent : IEvent
{
    private string? _displayName;
    private RuleOrdering? _ordering;

    internal RuleDescriptor Build()
    {
        var descriptor = new RuleDescriptor
        {
            Key = _key,
            Event = _event,
            DisplayName = _displayName,
            Ordering = _ordering ?? RuleOrdering.BeforeAndAfter,
        };
        _services.AddKeyedSingleton(descriptor.Key, descriptor);
        return descriptor;
    }

    public RuleConfiguration<TEvent> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public RuleConfiguration<TEvent> WithOrdering(RuleOrdering ordering)
    {
        _ordering = ordering;
        return this;
    }

    public RuleConfiguration<TEvent> WithAction<TRule>() where TRule : class, IRule<TEvent>
    {
        _services.AddKeyedTransient<IRule<TEvent>, TRule>(_key);
        return this;
    }

    public RuleConfiguration<TEvent> WithAction(RuleAction<TEvent> action)
    {
        _services.AddKeyedTransient(_key, (sp, key) => action);
        _services.AddKeyedTransient<IRule<TEvent>>(_key, (sp, key) =>
        {
            var keyedDelegate = sp.GetRequiredKeyedService<RuleAction<TEvent>>(key);
            return new RuleActionHandler<TEvent>(keyedDelegate);
        });
        return this;
    }

    public RuleConfiguration<TEvent> WithCondition<TRule>() where TRule : class, IRule<TEvent>
    {
        _services.AddKeyedTransient<IRule<TEvent>, TRule>(_key);
        return this;
    }

    public RuleConfiguration<TEvent> WithCondition(RuleCondition condition)
    {
        _services.AddKeyedTransient(_key, (sp, key) => condition);
        _services.AddKeyedTransient<IRuleCondition>(_key, (sp, key) =>
        {
            var keyedDelegate = sp.GetRequiredKeyedService<RuleCondition>(key);
            return new RuleConditionHandler(keyedDelegate);
        });
        return this;
    }
}
