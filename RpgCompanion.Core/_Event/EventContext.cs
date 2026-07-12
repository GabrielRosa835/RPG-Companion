namespace RpgCompanion.Core;

using Toolbox;

public abstract class EventContext : ITrigger
{
    /// <summary>
    /// Grants access to scoped dependencies for the current pipeline.
    /// </summary>
    public abstract IRegistry Registry { get; }

    /// <summary>
    /// Grants access to contextual in-memory storage
    /// </summary>
    public abstract IStorage Storage { get; }

    /// <summary>
    /// Calmly stops the pipeline, calling Teardown on exit
    /// </summary>
    public abstract void Exit(EventResult result);

    /// <summary>
    /// Completely stops the pipeline by cancelling the token
    /// </summary>
    public abstract void Halt(EventResult result);

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    public abstract EventTask Raise(Event e, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Queues the next event to run in the pipeline
    /// </summary>
    public abstract void Next(Event e);
}
