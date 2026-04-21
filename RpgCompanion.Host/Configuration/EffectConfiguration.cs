namespace RpgCompanion.Host.Configuration;

using Core.Events;
using Delegates;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class EffectConfiguration<TEvent>(IServiceCollection _services, EffectKey _key, EventKey _eventKey) where TEvent : IEvent
{
    private string? _displayName;

    public EffectDescriptor Build()
    {
        var descriptor = new EffectDescriptor
        {
            Key = _key,
            Event = _eventKey,
            DisplayName = _displayName,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public EffectConfiguration<TEvent> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public EffectConfiguration<TEvent> WithAction(EffectAction<TEvent> action)
    {
        _services.AddKeyedTransient<EffectAction<TEvent>>(_key, (sp, key) => action);
        _services.AddKeyedTransient<IEffect<TEvent>>(_key, (sp, key) =>
        {
            var keyedDelegate = sp.GetRequiredKeyedService<EffectAction<TEvent>>(key);
            return new EffectActionHandler<TEvent>(keyedDelegate);
        });
        return this;
    }

    public EffectConfiguration<TEvent> WithAction<TEffect>() where TEffect : class, IEffect<TEvent>
    {
        _services.AddKeyedTransient<IEffect<TEvent>, TEffect>(_key);
        return this;
    }

    public EffectConfiguration<TEvent> WithCondition(EffectCondition<TEvent> condition)
    {
        _services.AddKeyedTransient<EffectCondition<TEvent>>(_key, (sp, key) => condition);
        _services.AddKeyedTransient<IEffectCondition<TEvent>>(_key, (sp, key) =>
        {
            var keyedDelegate = sp.GetRequiredKeyedService<EffectCondition<TEvent>>(key);
            return new EffectConditionHandler<TEvent>(keyedDelegate);
        });
        return this;
    }

    public EffectConfiguration<TEvent> WithCondition<TEffectCondition>()
        where TEffectCondition : class, IEffectCondition<TEvent>
    {
        _services.AddKeyedTransient<IEffectCondition<TEvent>, TEffectCondition>(_key);
        return this;
    }
}
