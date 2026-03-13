using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application;

public interface IEventComponentBuilder<TEvent> where TEvent : IEvent
{
    IEventComponentBuilder<TEvent> AddRule<TRule> (RuleOrdering ordering = RuleOrdering.Both) where TRule : IRule<TEvent>;
    IEventComponentBuilder<TEvent> AddEffect<TEffect> () where TEffect : IEffect<TEvent>;
    IEventComponentBuilder<TEvent> AddContract<TContract> () where TContract : IContract<TEvent>;
    IEventComponentBuilder<TEvent> AddPackager<TPackager> () where TPackager : IPackager<TEvent>;
}