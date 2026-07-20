namespace RpgCompanion.Host.Configuration;

internal class InitializationContext(
    IServiceScope _scope,
    CancellationTokenSource _cancellationSource)
    : IInitializationContextAsync, IDisposable, IAsyncDisposable
{
    public IRegistry Registry { get; } = new Registry(_scope.ServiceProvider);
    public CancellationToken CancellationToken => _cancellationSource.Token;
    public InitializationExecutor Executor { get; set; }

    public void Dispose()
    {
        _scope.Dispose();
        _cancellationSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_scope);
        await CastAndDispose(_cancellationSource);
    }

    private static async ValueTask CastAndDispose(IDisposable resource)
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
