namespace RpgCompanion.Application;

using Core.Engine;
using Core.Events;
using Core.Meta;
using Microsoft.Extensions.DependencyInjection;
using Services;

internal class EventBuilder<TEvent>(IServiceCollection services, ComponentCollection components) : IEventBuilder<TEvent> where TEvent : IEvent
{
    private EventDescriptor _descriptor = new();
    internal EventDescriptor Build() => _descriptor;

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

    public IEventBuilder<TEvent> AddBundler(Action<IBundlerBuilder<TEvent>> configure)
    {
        var builder = new BundlerBuilder<TEvent>(services);
        configure(builder);
        components.Bundlers.Add(builder.Build());
        return this;
    }
}
