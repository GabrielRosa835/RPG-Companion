using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events;

public interface IEffectChecker<in TEvent, TEffect> where TEvent : IEvent where TEffect : IEffect<TEvent>
{
   bool ShouldApply (TEvent @event, IContext context);
   public static IEffectChecker<TEvent, TEffect> Of (Func<TEvent, IContext, bool> shouldApply)
      => new EffectCheckerWrapper<TEvent, TEffect>(shouldApply);
}

internal record EffectCheckerWrapper<TEvent, TEffect> (
   Func<TEvent, IContext, bool> shouldApply)
   : IEffectChecker<TEvent, TEffect>
   where TEvent : IEvent 
   where TEffect : IEffect<TEvent>
{
   public bool ShouldApply (TEvent @event, IContext context) => shouldApply(@event, context);
}