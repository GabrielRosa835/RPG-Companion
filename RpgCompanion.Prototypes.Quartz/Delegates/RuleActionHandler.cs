namespace RpgCompanion.QuartzPrototype.Delegates;

using Core.Engine.Contexts;
using Core.Events;

public class RuleActionHandler<TEvent>(RuleAction<TEvent> action) : IRule<TEvent> where TEvent : IEvent
{
    public TEvent Apply(IContext context) => action(context);
}

public class RuleActionHandler<TEvent, TTrigger>(RuleAction<TEvent, TTrigger> action)
    : IRule<TEvent, TTrigger> where TEvent : IEvent where TTrigger : IEvent
{
    public TEvent Apply(IContext context) => action(context);
}
