namespace RpgCompanion.Host;

internal class IntentContext : IIntentContext, IDisposable, IAsyncDisposable
{
    public IntentContext(ScopeProvider scopeProvider)
    {
        Scope = scopeProvider.CreateScope();
        Registry = Scope.ServiceProvider.GetRequiredService<IRegistry>();
    }

    internal IServiceScope Scope { get; }
    public IRegistry Registry { get; }

    internal CancellationTokenSource CancellationSource { get; set; } = default!;
    public CancellationToken CancellationToken => CancellationSource.Token;

    public void Dispose()
    {
        Scope.Dispose();
        CancellationSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(Scope);
        await CastAndDispose(CancellationSource);
    }

    private static ValueTask CastAndDispose(IDisposable resource)
    {
        if (resource is IAsyncDisposable resourceAsyncDisposable)
        {
            return resourceAsyncDisposable.DisposeAsync();
        }
        resource.Dispose();
        return ValueTask.CompletedTask;
    }
}
