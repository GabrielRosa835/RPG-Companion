namespace RpgCompanion.Core;

using Toolbox;

public abstract class EventContext : IEventTrigger
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
    /// The pipeline's CancellationToken
    /// </summary>
    public abstract CancellationToken CancellationToken { get; }

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    public abstract EventTask Raise(Event e, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completely stops the pipeline by cancelling the token
    /// </summary>
    public abstract void Halt();
}
