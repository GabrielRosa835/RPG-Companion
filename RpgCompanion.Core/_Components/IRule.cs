namespace RpgCompanion.Core;

using Engine.Contexts;

public interface IRule<TEvent> where TEvent : IEvent
{
    public TEvent Apply(IContext context);
}

public delegate TEvent RuleAction<TEvent>(IContext context) where TEvent : IEvent;
