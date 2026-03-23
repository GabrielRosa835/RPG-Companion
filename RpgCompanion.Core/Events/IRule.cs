namespace RpgCompanion.Core.Events.Producers;

using Contexts;

public delegate bool RuleCondition<in TEvent>(IContext context) where TEvent : IEvent;
public delegate IEvent RuleAction<in TEvent>(IContext context) where TEvent : IEvent;

public interface IRule<in TEvent> where TEvent : IEvent
{
    public bool ShouldApply(IContext context);
    public IEvent Apply(IContext context);
}
