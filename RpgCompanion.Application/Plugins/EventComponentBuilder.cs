using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application;

internal class EventComponentBuilder<TEvent>(ComponentCollection components) : IEventComponentBuilder<TEvent> where TEvent : IEvent
{
    public IEventComponentBuilder<TEvent> AddRule<TRule> (RuleOrdering ordering = RuleOrdering.Both) where TRule : IRule<TEvent>
    {
        components.AddRule<TRule, TEvent>(ordering);
        return this;
    }
    public IEventComponentBuilder<TEvent> AddEffect<TEffect> () where TEffect : IEffect<TEvent>
    {
        components.AddEffect<TEffect, TEvent>();
        return this;
    }
    public IEventComponentBuilder<TEvent> AddContract<TContract> () where TContract : IContract<TEvent>
    {
        components.AddContract<TContract, TEvent>();
        return this;
    }
    public IEventComponentBuilder<TEvent> AddPackager<TPackager> () where TPackager : IPackager<TEvent>
    {
        components.AddPackager<TPackager, TEvent>();
        return this;
    }
}