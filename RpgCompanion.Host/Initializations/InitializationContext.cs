namespace RpgCompanion.Host.Configuration;

using HostExclusive;

internal class InitializationContext(
    IServiceScope _scope,
    HostContext _hostContext,
    Registry _registry,
    CancellationTokenSource _cancellationSource)
    : IInitializationContext, IDisposable, IAsyncDisposable
{
    public IRegistry Registry => _registry;
    public IHostContext Host => _hostContext;

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
