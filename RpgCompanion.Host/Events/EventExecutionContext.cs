namespace RpgCompanion.Host.Events;

using Core;

internal class EventExecutionContext : IDisposable
{
    internal readonly object Lock = new();

    public EventExecutionContext(EventContextImpl eventContext)
    {
        Context = eventContext;
        eventContext.ExecutionContext = this;
    }

    internal Task<EventResult> Task { get; set; } = default!;
    internal Event CurrentEvent { get; set; } = default!;
    internal TimeSpan? SleepTime { get; set; }

    internal CancellationTokenSource CancellationSource { get; init; } = default!;
    internal EventEngine Engine { get; init; } = default!;
    internal IServiceScope ServiceScope { get; init; } = default!;

    internal Guid Id { get; } = Guid.NewGuid();
    internal EventContextImpl Context { get; }

    internal StopRequest Stopping
    {
        get
        {
            lock (Lock) return field;
        }
        private set
        {
            lock (Lock) field = value;
        }
    } = new StopRequest.None();

    internal Event? NextEvent
    {
        get
        {
            lock (Lock) return field;
        }
        set
        {
            lock (Lock) field = value;
        }
    }

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    public EventTask Raise(Event e, CancellationToken? cancellationToken = null) => Engine.Raise(e);

    /// <summary>
    /// Calmly stops the pipeline, calling Teardown on exit
    /// </summary>
    public void Exit(EventResult result) => Stopping = new StopRequest.Exiting(result);

    /// <summary>
    /// Stops the pipeline by cancelling the token
    /// </summary>
    internal void Halt(EventResult result)
    {
        CancellationSource.Cancel();
        Stopping = new StopRequest.Halting(result);
    }

    /// <summary>
    /// Stops the pipeline by cancelling the token and throws an exception to stop current scope execution.
    /// Such exception is silently captured
    /// </summary>
    internal void Terminate(EventResult result)
    {
        CancellationSource.Cancel();
        Stopping = new StopRequest.Terminating(result);
        throw new OperationCanceledException();
    }

    /// <summary>
    /// Queues the next event to run in the pipeline
    /// </summary>
    public void Next(Event e)
    {
        NextEvent = e;
    }

    public void Dispose()
    {
        ServiceScope.Dispose();
        CancellationSource?.Dispose();
    }
}
