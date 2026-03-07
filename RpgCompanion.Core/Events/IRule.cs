using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Events.Producers;

public interface IRule<out TEvent> where TEvent : IEvent
{
   TEvent Apply (IContext context);

   public static IRule<TEvent> Of(Func<IContext, TEvent> apply) => new RuleWrapper<TEvent>(apply);
}

internal readonly record struct RuleWrapper<TEvent> (Func<IContext, TEvent> apply) : IRule<TEvent> where TEvent : IEvent
{
   public TEvent Apply(IContext context) => apply(context);
}
