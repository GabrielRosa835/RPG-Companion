namespace RpgCompanion.Host;

using Toolbox.Storage;

internal class EventContext : IEventContext
{
    internal required EventExecutionContext ExecutionContext { get; init; }
    public required IRegistry Registry { get; init; }
    public required IStorage Storage { get; init; }

    public CancellationToken CancellationToken => ExecutionContext.CancellationSource.Token;

    public Task<EventResult> Raise(Event e, CancellationToken cancellationToken = default)
    {
        return ExecutionContext.Engine.Raise(e, cancellationToken);
    }

    public void Halt(bool throwException = false)
    {
        ExecutionContext.CancellationSource.Cancel();
        if (throwException) CancellationToken.ThrowIfCancellationRequested();
    }

    public void Exit(EventResult result) => ExecutionContext.Result = result;
    public void Continue(Event nextEvent) => ExecutionContext.Next = nextEvent;

    public void Continue<TEvent>() where TEvent : Event
    {
        Event next = ExecutionContext.Factory.Create<TEvent>();
        next ??= ExecutionContext.Factory.Create(typeof(TEvent));
        ExecutionContext.Next = next ?? throw new ArgumentNullException(nameof(next));
    }
}
