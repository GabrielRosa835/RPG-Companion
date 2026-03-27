namespace RpgCompanion.Application;

using Core.Events;
using Core.Events.Producers;
using Core.Meta;
using Microsoft.Extensions.DependencyInjection;
using Services;

internal class RuleBuilder<TEvent>(IServiceCollection services) : IRuleBuilder<TEvent> where TEvent : IEvent
{
    private RuleDescriptor _descriptor = new();
    public RuleDescriptor Build()
    {
        _descriptor.Condition ??= (RuleCondition<TEvent>) (_ => true);
        _descriptor.EventType = typeof(TEvent);
        return _descriptor;
    }

    public IRuleBuilder<TEvent> WithOrdering(RuleOrdering ordering)
    {
        _descriptor.Ordering = ordering;
        return this;
    }

    public IRuleBuilder<TEvent> WithComponent<TRule>() where TRule : class, IRule<TEvent>
    {
        services.AddTransient<TRule>();
        services.AddTransient<IRule<TEvent>, TRule>();
        var serviceDescriptor = services.Last();
        _descriptor.Service = serviceDescriptor;
        return this;
    }

    public IRuleBuilder<TEvent> WithAction(RuleAction<TEvent> action)
    {
        _descriptor.Action = action;
        return this;
    }

    public IRuleBuilder<TEvent> WithCondition(RuleCondition<TEvent> condition)
    {
        _descriptor.Condition = condition;
        return this;
    }
}
