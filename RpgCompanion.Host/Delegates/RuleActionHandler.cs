namespace RpgCompanion.Host.Delegates;

using Core;
using Core.Engine.Contexts;
using Core.Events;

public class RuleActionHandler<TEvent>(RuleAction<TEvent> action) : IRule<TEvent> where TEvent : IEvent
{
    public TEvent Apply(IContext context) => action(context);
}
