namespace RpgCompanion.Host;

using Common;

internal class IntentContext : IAsyncIntentContext, IDisposable, IAsyncDisposable
{
    internal required CancellationTokenSource CancellationSource { get; init; }
    public required IRegistry Registry { get; init; }
    public CancellationToken CancellationToken => CancellationSource.Token;

    public void Dispose()
    {
        CancellationSource.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return this.CastAndDispose(CancellationSource);
    }
}
