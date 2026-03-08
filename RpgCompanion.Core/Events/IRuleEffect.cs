using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events.Producers;


public interface IRule<out TEvent, TEffect> where TEvent : IEvent where TEffect : IEffect<TEvent>
{
   TEvent Apply (IContextSnapshot context);

   public static IRule<TEvent> Of (Func<IContextSnapshot, TEvent> apply) => new RuleWrapper<TEvent>(apply);
}

internal record RuleEffectWrapper<TEvent, TEffect> (Func<IContextSnapshot, TEvent> apply) : IRule<TEvent, TEffect> 
   where TEvent : IEvent
   where TEffect : IEffect<TEvent>
{
   public TEvent Apply (IContextSnapshot context) => apply(context);
}