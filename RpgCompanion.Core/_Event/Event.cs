namespace RpgCompanion.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

public abstract class Event
{
    protected ValueTask Completed => ValueTask.CompletedTask;
    public virtual TimeSpan? SleepTime { get; } = null!;
    public virtual ValueTask SetupAsync(EventContext ctx, CancellationToken ct) => ValueTask.CompletedTask;
    public virtual ValueTask ExecuteAsync(EventContext ctx, CancellationToken ct) => ValueTask.CompletedTask;
    public virtual ValueTask TeardownAsync(EventContext ctx, CancellationToken ct) => ValueTask.CompletedTask;
}

public class EventExample : Event
{
    public override ValueTask ExecuteAsync(EventContext ctx, CancellationToken ct)
    {
        return Completed;
    }
}
