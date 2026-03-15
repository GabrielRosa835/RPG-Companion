namespace RpgCompanion.Application;

using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

public interface IEventComponentBuilder<TEvent> where TEvent : IEvent
{
    public IEventComponentBuilder<TEvent> AddRule<TRule>(RuleOrdering ordering = RuleOrdering.Both) where TRule : IRule<TEvent>;
    public IEventComponentBuilder<TEvent> AddEffect<TEffect>() where TEffect : IEffect<TEvent>;
    public IEventComponentBuilder<TEvent> AddPackager<TPackager>() where TPackager : IPackager<TEvent>;
}
