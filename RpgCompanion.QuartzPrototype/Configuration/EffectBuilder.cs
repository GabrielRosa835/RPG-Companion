namespace RpgCompanion.QuartzPrototype.Configuration;

using Core.Events;
using Delegates;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class EffectBuilder<TEvent>(IServiceCollection services, string? key) where TEvent : IEvent
{
    private EffectDescriptor _descriptor = new(key ?? Guid.NewGuid().ToString());

    public EffectDescriptor Build()
    {
        services.AddKeyedSingleton(_descriptor.Key, _descriptor);
        return _descriptor;
    }

    public EffectBuilder<TEvent> WithName(string name)
    {
        _descriptor.DisplayName = name;
        return this;
    }

    public EffectBuilder<TEvent> WithAction(EffectAction<TEvent> action)
    {
        services.AddKeyedTransient<EffectAction<TEvent>>(_descriptor.Key, (sp, key) => action);
        services.AddKeyedTransient<IEffect<TEvent>, EffectActionHandler<TEvent>>(_descriptor.Key);
        return this;
    }

    public EffectBuilder<TEvent> WithAction<TEffect>() where TEffect : class, IEffect<TEvent>
    {
        services.AddKeyedTransient<IEffect<TEvent>, TEffect>(_descriptor.Key);
        return this;
    }

    public EffectBuilder<TEvent> WithCondition(EffectCondition<TEvent> condition)
    {
        services.AddKeyedTransient<EffectCondition<TEvent>>(_descriptor.Key, (sp, key) => condition);
        services.AddKeyedTransient<IEffectCondition<TEvent>, EffectConditionHandler<TEvent>>(_descriptor.Key);
        return this;
    }

    public EffectBuilder<TEvent> WithCondition<TEffectCondition>()
        where TEffectCondition : class, IEffectCondition<TEvent>
    {
        services.AddKeyedTransient<IEffectCondition<TEvent>, TEffectCondition>(_descriptor.Key);
        return this;
    }
}
