namespace RpgCompanion.Core.Engine;

using Events;

public interface IPipeline<TEventIn> where TEventIn : IEvent
{
    public IPipeline<TEventOut> FollowedBy<TEventOut>(Continuation<TEventIn, TEventOut> continuation)
        where TEventOut : IEvent;
}


public interface IPipeline
{
    public IPipeline<TEvent> Raise<TEvent>(TEvent e) where TEvent : IEvent;
}

public delegate TEventOut Continuation<TEventIn, TEventOut>(TEventIn e);
