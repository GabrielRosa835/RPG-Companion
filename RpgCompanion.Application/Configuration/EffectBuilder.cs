namespace RpgCompanion.Application;

using Core.Events;
using Core.Meta;
using Microsoft.Extensions.DependencyInjection;
using Services;

internal class EffectBuilder<TEvent> (IServiceCollection services) : IEffectBuilder<TEvent> where TEvent : IEvent
{
    private EffectDescriptor _descriptor = new();
    public EffectDescriptor Build()
    {
        _descriptor.Condition ??= (EffectCondition<TEvent>) (_ => true);
        _descriptor.EventType = typeof(TEvent);
        return _descriptor;
    }

    public IEffectBuilder<TEvent> WithComponent<TEffect>() where TEffect : class, IEffect<TEvent>
    {
        services.AddTransient<TEffect>();
        services.AddTransient<IEffect<TEvent>, TEffect>();
        var serviceDescriptor = services.Last();
        _descriptor.Service = serviceDescriptor;
        return this;
    }

    public IEffectBuilder<TEvent> WithAction(EffectAction<TEvent> action)
    {
        _descriptor.Action = action;
        return this;
    }

    public IEffectBuilder<TEvent> WithCondition(EffectCondition<TEvent> condition)
    {
        _descriptor.Condition = condition;
        return this;
    }
}
