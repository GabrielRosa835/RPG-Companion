namespace RpgCompanion.QuartzPrototype.Configuration;

using Core.Events;
using Delegates;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class RuleBuilder<TEvent>(IServiceCollection services, string? key) where TEvent : IEvent
{
    private readonly RuleDescriptor _descriptor = new(key ?? Guid.NewGuid().ToString());

    internal RuleDescriptor Build()
    {
        services.AddKeyedSingleton(_descriptor.Key, _descriptor);
        return _descriptor;
    }

    public RuleBuilder<TEvent> WithOrdering(RuleOrdering ordering)
    {
        _descriptor.Ordering = ordering;
        return this;
    }

    public RuleBuilder<TEvent> WithAction<TRule>() where TRule : class, IRule<TEvent>
    {
        services.AddKeyedTransient<IRule<TEvent>, TRule>(_descriptor.Key);
        return this;
    }

    public RuleBuilder<TEvent> WithAction(RuleAction<TEvent> action)
    {
        services.AddKeyedTransient(_descriptor.Key, (sp, key) => action);
        services.AddKeyedTransient<IRule<TEvent>, RuleActionHandler<TEvent>>(_descriptor.Key);
        return this;
    }

    public RuleBuilder<TEvent> WithCondition<TRule>() where TRule : class, IRule<TEvent>
    {
        services.AddKeyedTransient<IRule<TEvent>, TRule>(_descriptor.Key);
        return this;
    }

    public RuleBuilder<TEvent> WithCondition(RuleCondition condition)
    {
        services.AddKeyedTransient(_descriptor.Key, (sp, key) => condition);
        services.AddKeyedTransient<IRuleCondition, RuleConditionHandler<TEvent>>(_descriptor.Key);
        return this;
    }
}
