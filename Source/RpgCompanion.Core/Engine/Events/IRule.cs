using RpgCompanion.Core.Engine.Contexts;

namespace RpgCompanion.Core.Events;

public delegate bool RuleCondition(IContext context);
public delegate TEvent RuleAction<out TEvent>(IContext context) where TEvent : IEvent;

public interface IRule<out TEvent> where TEvent : IEvent
{
    public TEvent Apply(IContext context);
}

public interface IRuleCondition
{
    public bool ShouldApply(IContext e);
}
