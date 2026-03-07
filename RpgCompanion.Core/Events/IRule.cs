using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Events.Producers;

public interface IRule<out TEvent> where TEvent : IEvent
{
   TEvent Apply (IContext context);

   public static IRule<TEvent> Of(RuleDelegate<TEvent> ruleDelegate) => new RuleWrapper<TEvent>(ruleDelegate);
   
}
public delegate TEvent RuleDelegate<out TEvent>(IContext context) where TEvent : IEvent;

internal readonly record struct RuleWrapper<TEvent> (RuleDelegate<TEvent> rule) : IRule<TEvent> where TEvent : IEvent
{
   public TEvent Apply(IContext context) => rule(context);
}
