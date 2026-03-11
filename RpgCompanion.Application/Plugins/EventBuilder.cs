using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application;

public interface IEventBuilder<TEvent> where TEvent : IEvent
{
    public IEventBuilder<TEvent> AddRule<TRule>() where TRule : IRule<TEvent>;
    public IEventBuilder<TEvent> AddEffect<TEffect>() where TEffect : IEffect<TEvent>;
    public IEventBuilder<TEvent> AddContract<TContract>() where TContract : IContract<TEvent>;
    public IEventBuilder<TEvent> AddPackager<TTemplate>() where TTemplate : IPackager<TEvent>;
}

internal class EventBuilder<TEvent>(ComponentCollection components, PluginBuilder previous) : IEventBuilder<TEvent> where TEvent : IEvent
{
    public IEventBuilder<TEvent> AddRule<TRule>() where TRule : IRule<TEvent>
    {
        components.AddRule<TRule, TEvent>();
        return this;
    }
    public IEventBuilder<TEvent> AddEffect<TEffect>() where TEffect : IEffect<TEvent>
    {
        components.AddEffect<TEffect, TEvent>();
        return this;
    }
    public IEventBuilder<TEvent> AddContract<TContract>() where TContract : IContract<TEvent>
    {
        components.AddContract<TContract, TEvent>();
        return this;
    }
    public IEventBuilder<TEvent> AddPackager<TPackager>() where TPackager : IPackager<TEvent>
    {
        components.AddPackager<TPackager, TEvent>();
        return this;
    }
}