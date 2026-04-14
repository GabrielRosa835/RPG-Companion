namespace RpgCompanion.QuartzPrototype;

using Core.Engine.Contexts;
using Core.Events;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal interface IRuleWrapper<out TEvent, TRule> : IRule<TEvent> where TRule : IRule<TEvent> where TEvent : IEvent;

internal class RuleTriggerBuilder<TEvent>(IServiceProvider serviceProvider, IContext context) where TEvent : IEvent
{
    public ITrigger Trigger(TriggerBuilder builder, string ruleKey)
    {
        var descriptor = serviceProvider.GetRequiredKeyedService<RuleDescriptor>(ruleKey);



        if (condition is not null && !condition.ShouldApply(context))
        {
            return null;
        }
        var e = rule.Apply(context);
        return builder.ForJob(e.ToJobKey())
            .UsingJobData(new JobDataMap { ["event"] = e })
            .StartNow()
            .Build();
    }
}
