using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events.Producers;

public interface IRuleChecker<out TEvent, TRule> where TEvent : IEvent where TRule : IRule<TEvent>
{
   public bool ShouldApply (IContextSnapshot context);

   public static IRuleChecker<TEvent, TRule> Of (Func<IContextSnapshot, bool> shouldApply) 
      => new RuleCheckerWrapper<TEvent, TRule>(shouldApply);
}

internal record RuleCheckerWrapper<TEvent, TRule> (Func<IContextSnapshot, bool> shouldApply)
   : IRuleChecker<TEvent, TRule> where TEvent : IEvent where TRule : IRule<TEvent>
{
   public bool ShouldApply (IContextSnapshot context) => shouldApply(context);
}