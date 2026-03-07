using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Events;

public interface IEffectChecker<in TEvent> where TEvent : IEvent
{
   bool ShouldApply (TEvent @event, IContext context);
   public static IEffectChecker<TEvent> Of (Func<TEvent, IContext, bool> shouldApply)
      => new EffectCheckerWrapper<TEvent>(shouldApply);
}

internal readonly record struct EffectCheckerWrapper<TEvent> (
   Func<TEvent, IContext, bool> shouldApply)
   : IEffectChecker<TEvent> where TEvent : IEvent
{
   public bool ShouldApply (TEvent @event, IContext context) => shouldApply(@event, context);
}