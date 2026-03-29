using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

internal class RuleBuilder<TEvent>(IServiceCollection services) : IRuleBuilder<TEvent> where TEvent : IEvent
{
    private readonly RuleDescriptor _descriptor = new();
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
        services.AddScoped<TRule>();
        services.AddScoped<IRule<TEvent>, TRule>();
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
