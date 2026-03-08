using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events.Producers;

public interface IRule<out TEvent> where TEvent : IEvent
{
   TEvent Apply (IContextSnapshot context);

   public static IRule<TEvent> Of(Func<IContextSnapshot, TEvent> apply) => new RuleWrapper<TEvent>(apply);
}

internal record RuleWrapper<TEvent> (Func<IContextSnapshot, TEvent> apply) : IRule<TEvent> where TEvent : IEvent
{
   public TEvent Apply(IContextSnapshot context) => apply(context);
}
