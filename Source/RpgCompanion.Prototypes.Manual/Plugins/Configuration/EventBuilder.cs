using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

internal class EventBuilder<TEvent>(IServiceCollection services, ComponentCollection components) : IEventBuilder<TEvent> where TEvent : IEvent
{
    private EventDescriptor _descriptor = new();
    internal EventDescriptor Build()
    {
        _descriptor.Type = typeof(TEvent);
        components.Events.Add(_descriptor);
        return _descriptor;
    }

    public IEventBuilder<TEvent> WithName(string name)
    {
        _descriptor.DisplayName = name;
        return this;
    }

    public IEventBuilder<TEvent> WithPriority(int priority)
    {
        _descriptor.Priority = priority;
        return this;
    }

    public IEventBuilder<TEvent> AddRule(Action<IRuleBuilder<TEvent>> configure)
    {
        var builder = new RuleBuilder<TEvent>(services);
        configure(builder);
        components.Rules.Add(builder.Build());
        return this;
    }

    public IEventBuilder<TEvent> AddEffect(Action<IEffectBuilder<TEvent>> configure)
    {
        var builder = new EffectBuilder<TEvent>(services);
        configure(builder);
        components.Effects.Add(builder.Build());
        return this;
    }
}
