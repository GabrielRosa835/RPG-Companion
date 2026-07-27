namespace RpgCompanion.Host.Configuration;

internal class InitializationContext(
    IServiceScope _scope,
    CancellationTokenSource _cancellationSource,
    IRegistry _registry)
    : IInitializationContext, IDisposable, IAsyncDisposable
{
    public IRegistry Registry => _registry;

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
