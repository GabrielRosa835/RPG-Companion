namespace RpgCompanion.Host;

using Common;

internal class InitializationContext : IInitializationContext, IDisposable, IAsyncDisposable
{
    internal required IServiceScope Scope { get; init; }
    internal required CancellationTokenSource CancellationSource { get; init; }
    public required IRegistry Registry { get; init; }

    public void Dispose()
    {
        Scope.Dispose();
        CancellationSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await this.CastAndDispose(Scope);
        await this.CastAndDispose(CancellationSource);
    }
}
