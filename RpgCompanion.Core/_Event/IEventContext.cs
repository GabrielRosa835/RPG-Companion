namespace RpgCompanion.Core;

using Toolbox;

public interface IEventContext : IEventTrigger
{
    /// <summary>
    /// Grants access to scoped dependencies for the current pipeline.
    /// </summary>
    IRegistry Registry { get; }

    /// <summary>
    /// Grants access to contextual in-memory storage
    /// </summary>
    IStorage Storage { get; }

    /// <summary>
    /// The pipeline's CancellationToken
    /// </summary>
    CancellationToken CancellationToken { get; }

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    EventTask Raise(Event e, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completely stops the pipeline by cancelling the token
    /// </summary>
    void Halt();
}
