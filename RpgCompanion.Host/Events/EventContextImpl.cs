namespace RpgCompanion.Host.Events;

using Core;
using Toolbox;

public class EventContextImpl(IRegistry _registry, IStorage _storage) : EventContext
{
    internal EventExecutionContext ExecutionContext { get; set; } = default!;

    /// <summary>
    /// Grants access to scoped dependencies for the current pipeline.
    /// </summary>
    public override IRegistry Registry => _registry;

    /// <summary>
    /// Grants access to contextual in-memory storage
    /// </summary>
    public override IStorage Storage => _storage;

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    public override EventTask Raise(Event e, CancellationToken cancellationToken = default) =>
        ExecutionContext.Engine.Raise(e, cancellationToken);

    /// <summary>
    /// Completely stops the pipeline by cancelling the token
    /// </summary>
    public override void Halt() => ExecutionContext.CancellationSource?.Cancel();

    public override CancellationToken CancellationToken => ExecutionContext.CancellationSource.Token;
}
