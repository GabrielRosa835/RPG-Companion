namespace RpgCompanion.Host;

using Configuration;

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
            return InitializationResult.Faulted(new InvalidOperationException("Plugin metadata is not loaded"));
        }
        try
        {
            Console.WriteLine($"Initializing plugin {metadata.Resource}");

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

            IInitializationResult result = InitializationResult.None;

            await using var context = new InitializationContext(scope, cts, new Registry(scope.ServiceProvider));

            if (asyncInitialization is not null)
            {
                await asyncInitialization.Initialize(context, cancellationToken);
                result = InitializationResult.Completed(true);
                metadata.Initialized = true;
            }
            else if (syncInitialization is not null)
            {
                syncInitialization!.Initialize(context);
                result = InitializationResult.Completed(true);
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
