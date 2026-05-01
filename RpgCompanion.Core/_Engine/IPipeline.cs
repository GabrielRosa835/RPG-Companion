namespace RpgCompanion.Core;

// Scoped within a single event-processing session that delegates remaining continuations forward.
public interface IPipeline<TEventIn> where TEventIn : IEvent
{
    public IPipeline<TEventOut> FollowedBy<TEventOut>(Func<TEventIn, TEventOut> continuation)
        where TEventOut : IEvent;
}
