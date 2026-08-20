namespace RpgCompanion.Host;

using Common;

internal class EventExecutionContext : IDisposable, IAsyncDisposable
{
    internal readonly object Lock = new();

    internal required EventEngine Engine { get; init; }
    internal required IEventFactory Factory { get; init; }
    internal required IServiceScope ServiceScope { get; init; }
    internal required EventContext Context { get; init; }
    internal required CancellationTokenSource CancellationSource { get; init; }

    internal Guid Id { get; } = Guid.NewGuid();
    internal Task<EventResult> Task { get; set; } = default!;
    internal Event Current { get; set; } = default!;

    internal Event? Next { get; set; }
    internal bool Continuing => Next is not null || Exiting;
    internal EventResult? Result { get; set; }
    internal bool Exiting => Result is not null;

    public void Dispose()
    {
        CancellationSource.Dispose();
        ServiceScope.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await this.CastAndDispose(CancellationSource);
        await this.CastAndDispose(ServiceScope);
    }
}
