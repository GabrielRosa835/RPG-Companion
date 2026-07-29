namespace RpgCompanion.Host;

using Configuration;
using HostExclusive;

internal class PluginInitializer
{
    internal async Task<List<IInitializationResult>> InitializeMany(IEnumerable<PluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        var initializationTasks = plugins.Select(p => InitializeSingle(p, cancellationToken)).ToList();
        await Task.WhenAll(initializationTasks);
        return initializationTasks.Select(t => t.Result).ToList();
    }

    internal async Task<IInitializationResult> InitializeSingle(PluginMetadata metadata, CancellationToken cancellationToken = default)
    {
        if (!metadata.Loaded)
        {
            return InitializationResult.Faulted(new InvalidOperationException("Plugin is not loaded yet"));
        }
        try
        {
            IAsyncInitialization? asyncInitialization = metadata.Services.GetService<IAsyncInitialization>();
            IInitialization? syncInitialization = default!;

            if (asyncInitialization is null)
            {
                syncInitialization = metadata.Services.GetService<IInitialization>();
            }
            if (asyncInitialization is null && syncInitialization is null)
            {
                return InitializationResult.NoInitializationFound;
            }

            var scopeFactory = metadata.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateAsyncScope();
            var cts = cancellationToken.CanBeCanceled
                ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
                : new CancellationTokenSource();

            var registry = new Registry(scope.ServiceProvider);
            var hostContext = scope.ServiceProvider.GetRequiredService<HostContext>();
            await using var context = new InitializationContext(scope, hostContext, registry, cts);

            IInitializationResult result = InitializationResult.None;

            if (asyncInitialization is not null)
            {
                await asyncInitialization.Initialize(context, cancellationToken);
                result = InitializationResult.Completed(metadata, true);
                metadata.Initialized = true;
            }
            else if (syncInitialization is not null)
            {
                syncInitialization!.Initialize(context);
                result = InitializationResult.Completed(metadata, true);
                metadata.Initialized = true;
            }

            return result;
        }
        catch (Exception e)
        {
            return InitializationResult.Faulted(e);
        }
    }
}
