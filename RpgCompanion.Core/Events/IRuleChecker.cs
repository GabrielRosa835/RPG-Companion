using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Events.Producers;

public interface IRuleChecker<out TEvent> where TEvent : IEvent
{
   public bool ShouldApply (IContext context);

   public static IRuleChecker<TEvent> Of (Func<IContext, bool> shouldApply) 
      => new RuleCheckerWrapper<TEvent>(shouldApply);
}

internal readonly record struct RuleCheckerWrapper<TEvent> (Func<IContext, bool> shouldApply)
   : IRuleChecker<TEvent> where TEvent : IEvent
{
   public bool ShouldApply (IContext context) => shouldApply(context);
}