namespace RpgCompanion.Host;

using System.Threading;
using Microsoft.Extensions.DependencyInjection;

public class ScopeProvider(IServiceScopeFactory factory)
{
    private static readonly AsyncLocal<AsyncServiceScope?> _currentScope = new();

    public AmbientServiceScope CreateScope()
    {
        // 1. Join an existing scope (Not the owner)
        if (_currentScope.Value.HasValue)
        {
            return new AmbientServiceScope(
                innerScope: _currentScope.Value.Value,
                isOwner: false,
                onDispose: null);
        }

        // 2. Create a new root scope (Becomes the owner)
        var newScope = factory.CreateAsyncScope();
        _currentScope.Value = newScope;

        return new AmbientServiceScope(
            innerScope: newScope,
            isOwner: true,
            onDispose: () => _currentScope.Value = null); // Clear the AsyncLocal to prevent memory leaks
    }
}
