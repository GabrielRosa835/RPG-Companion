namespace RpgCompanion.Application.Engine;

using Microsoft.Extensions.DependencyInjection;

// Singleton
// TODO: Implementar uma forma melhor de temporização
internal class ScopedServiceScope : IDisposable
{
    private readonly IServiceScopeFactory _factory;
    private readonly TimeSpan _cleanupInterval;
    private DateTime _lastRequest;
    private IServiceScope? _scope;

    internal IServiceProvider ServiceProvider => GetProvider();
    private IServiceProvider GetProvider()
    {
        if (_scope is not null && DateTime.UtcNow - _lastRequest <= _cleanupInterval)
        {
            return _scope.ServiceProvider;
        }
        _scope?.Dispose();
        _scope = _factory.CreateScope();
        _lastRequest = DateTime.UtcNow;
        return _scope.ServiceProvider;
    }

    public ScopedServiceScope(IServiceScopeFactory factory, TimeSpan? cleanupInterval = null)
    {
        _cleanupInterval = cleanupInterval ?? TimeSpan.FromSeconds(120);
        _lastRequest = DateTime.UtcNow;
        _factory = factory;
    }

    internal void Reset()
    {
        if (_scope is null) return;
        _scope?.Dispose();
        _scope = null;
    }
    private static void TimedReset(object? state)
    {
        var scope = state as ScopedServiceScope;
        scope?.Reset();
    }

    public void Dispose() => _scope?.Dispose();
}
