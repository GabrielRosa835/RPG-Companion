namespace RpgCompanion.QuartzPrototype;

using Core.Events;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class EffectJob<TEvent>(ComponentManager componentManager, IServiceProvider serviceProvider) : IJob where TEvent : IEvent
{
    public Task Execute(IJobExecutionContext context)
    {
        var effectKey = (string) context.MergedJobDataMap["effectKey"];
        var descriptor = componentManager.GetEffect(effectKey);

        IEffectCondition<TEvent>? condition = serviceProvider.GetKeyedService<IEffectCondition<TEvent>>(effectKey);

        var e = (TEvent) context.MergedJobDataMap["event"];
        if (condition is not null && !condition.ShouldApply(e))
        {
            return Task.CompletedTask;
        }

        IEffect<TEvent> effect = serviceProvider.GetRequiredKeyedService<IEffect<TEvent>>(effectKey);

        effect.Apply(e);
        context.Result = e;

        return Task.CompletedTask;
    }
}
