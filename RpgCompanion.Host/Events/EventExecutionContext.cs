namespace RpgCompanion.Host.Events;

using Toolbox;

internal class EventExecutionContext : IEventContext, IDisposable, IAsyncDisposable
{
    internal readonly object Lock = new();

    internal required CancellationTokenSource CancellationSource { get; init; }
    internal required EventEngine Engine { get; init; }
    internal required IServiceScope ServiceScope { get; init; }
    internal required IEventFactory Factory { get; init; }
    public required IRegistry Registry { get; init; }
    public required IStorage Storage { get; init; }

    public CancellationToken CancellationToken => CancellationSource.Token;
    internal Guid Id { get; } = Guid.NewGuid();
    internal EventTask Task { get; set; } = default!;
    internal Event Current { get; set; } = default!;

    internal Event? Next { get; set; }
    internal bool Continuing => Next is not null || Exiting;
    internal EventResult? Result { get; set; }
    internal bool Exiting => Result is not null;

    public Task<EventResult> Raise(Event e, CancellationToken cancellationToken = default) => Engine.Raise(e, cancellationToken);

    public void Halt(bool throwException = false)
    {
        CancellationSource.Cancel();
        if (throwException) CancellationToken.ThrowIfCancellationRequested();
    }

    public void Exit(EventResult result) => Result = result;
    public void Continue(Event nextEvent) => Next = nextEvent;

    public void Continue<TEvent>() where TEvent : Event
    {
        Event next = Factory.Create<TEvent>();
        next ??= Factory.Create(typeof(TEvent));
        Next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public void Dispose()
    {
        ServiceScope.Dispose();
        CancellationSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(CancellationSource);
        await CastAndDispose(ServiceScope);
    }

    static async ValueTask CastAndDispose(IDisposable resource)
    {
        if (resource is IAsyncDisposable resourceAsyncDisposable)
        {
            await resourceAsyncDisposable.DisposeAsync();
        }
        else
        {
            resource.Dispose();
        }
    }
}
