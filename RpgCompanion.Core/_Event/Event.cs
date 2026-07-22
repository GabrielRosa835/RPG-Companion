namespace RpgCompanion.Core;

public abstract class Event
{
    public virtual TimeSpan? ExecutionInterval { get; } = default!;

    /// <summary>
    /// Helper that provides simplified access to ValueTask.CompletedTask, for when synchronous operation is desired
    /// </summary>
    protected ValueTask Completed => ValueTask.CompletedTask;
    public virtual ValueTask Setup(IEventContext context, CancellationToken CancellationToken) => ValueTask.CompletedTask;
    public virtual ValueTask Execute(IEventContext context, CancellationToken CancellationToken) => ValueTask.CompletedTask;
    public virtual ValueTask Teardown(IEventContext context, CancellationToken CancellationToken) => ValueTask.CompletedTask;
}
