using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IPipeline<TEventIn> where TEventIn : IEvent
{
   public IPipeline<TEventOut> FollowedBy<TEventOut>(Func<TEventIn, TEventOut> sequence) 
       where TEventOut : IEvent;
}


public interface IPipeline
{
   public IPipeline<TEvent> Raise<TEvent>(TEvent @event) where TEvent : IEvent;
}