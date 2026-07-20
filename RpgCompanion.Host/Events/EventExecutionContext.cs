namespace RpgCompanion.Host.Events;

internal class EventExecutionContext : IDisposable
{
    internal readonly object Lock = new();

    public EventExecutionContext(EventContext eventContext)
    {
        Context = eventContext;
        eventContext.ExecutionContext = this;
    }

    internal EventTask Task { get; set; } = default!;
    internal Event Current { get; set; } = default!;

    internal CancellationTokenSource CancellationSource { get; init; } = default!;
    internal EventEngine Engine { get; init; } = default!;
    internal IServiceScope ServiceScope { get; init; } = default!;

    internal Guid Id { get; } = Guid.NewGuid();
    internal EventContext Context { get; }

    public void Dispose()
    {
        ServiceScope.Dispose();
        CancellationSource.Dispose();
    }
}
