using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Events;

public interface IEffect<in TEvent> where TEvent : IEvent
{
   void Apply (TEvent @event, IContext context);
   public static IEffect<TEvent> Of(Action<TEvent, IContext> apply) => new EffectWrapper<TEvent>(apply);
}

internal readonly record struct EffectWrapper<TEvent> (Action<TEvent, IContext> apply) 
   : IEffect<TEvent> where TEvent : IEvent
{
   public void Apply (TEvent @event, IContext context) => apply(@event, context);
}