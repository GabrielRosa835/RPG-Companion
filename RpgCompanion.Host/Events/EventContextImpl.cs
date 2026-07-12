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
    /// Calmly stops the pipeline, calling Teardown on exit
    /// </summary>
    public override void Exit(EventResult result) => ExecutionContext.Exit(result);

    /// <summary>
    /// Completely stops the pipeline by cancelling the token
    /// </summary>
    public override void Halt(EventResult result) => ExecutionContext.Halt(result);

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    public override EventTask Raise(Event e, CancellationToken? cancellationToken = null) =>
        ExecutionContext.Raise(e, cancellationToken);

    /// <summary>
    /// Queues the next event to run in the pipeline
    /// </summary>
    public override void Next(Event e) => ExecutionContext.Next(e);
}
