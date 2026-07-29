namespace RpgCompanion.Host.Intents;

using HostExclusive;

internal class IntentContext(
    IServiceScope _scope,
    Registry _registry,
    HostContext _hostContext,
    CancellationTokenSource _cancellationSource)
    : IIntentContext, IDisposable, IAsyncDisposable
{
    public IRegistry Registry => _registry;
    public IHostContext Host => _hostContext;
    public CancellationToken CancellationToken => _cancellationSource.Token;

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
