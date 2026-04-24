using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

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
        if (_descriptor.HasService)
        {
            throw new InvalidOperationException("There can only be one component for an effect");
        }
        services.AddScoped<TEffect>();
        services.AddScoped<IEffect<TEvent>, TEffect>();
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
