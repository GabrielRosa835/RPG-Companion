using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Events;

public interface IEffect<in TEvent> where TEvent : IEvent
{
   void Apply (TEvent @event, IContext context);
   public static IEffect<TEvent> Of(HandlerDelegate<TEvent> handlerDelegate) => new EffectWrapper<TEvent>(handlerDelegate);
}
public delegate void HandlerDelegate<in TEvent>(TEvent @event, IContext context) where TEvent : IEvent;
internal readonly record struct EffectWrapper<TEvent>(HandlerDelegate<TEvent> handler) : IEffect<TEvent> where TEvent : IEvent
{
   public void Apply(TEvent @event, IContext context) => handler(@event, context);
}