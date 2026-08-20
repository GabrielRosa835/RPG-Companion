namespace RpgCompanion.Host;

/// <summary>
/// Wraps an AsyncServiceScope to prevent nested consumers from disposing the root scope.
/// </summary>
public sealed class AmbientServiceScope(
    AsyncServiceScope innerScope,
    bool isOwner,
    Action? onDispose) : IServiceScope, IAsyncDisposable
{
    public IServiceProvider ServiceProvider => innerScope.ServiceProvider;

    public void Dispose()
    {
        if (!isOwner) return;

        onDispose?.Invoke();
        innerScope.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (!isOwner) return;

        onDispose?.Invoke();
        await innerScope.DisposeAsync();
    }
}
