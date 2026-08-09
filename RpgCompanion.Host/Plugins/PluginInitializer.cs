namespace RpgCompanion.Host;

using Configuration;
using HostExclusive;

internal interface InitializationResult
{
    internal readonly record struct None : InitializationResult;

    internal readonly record struct Completed(PluginMetadata Metadata, bool WasAsync) : InitializationResult;

    internal readonly record struct Faulted(Exception Exception) : InitializationResult;

    internal readonly record struct NoInitializationFound : InitializationResult;
}

internal class PluginInitializer
{
    internal async Task<List<InitializationResult>> InitializeMany(IEnumerable<PluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        var initializationTasks = plugins.Select(p => InitializeSingle(p, cancellationToken)).ToList();
        await Task.WhenAll(initializationTasks);
        return initializationTasks.Select(t => t.Result).ToList();
    }

    internal async Task<InitializationResult> InitializeSingle(PluginMetadata metadata, CancellationToken cancellationToken = default)
    {
        if (!metadata.Loaded)
        {
            return new InitializationResult.Faulted(new InvalidOperationException("Plugin is not loaded yet"));
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
                return new InitializationResult.NoInitializationFound();
            }

            var scopeFactory = metadata.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateAsyncScope();
            var cts = cancellationToken.CanBeCanceled
                ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
                : new CancellationTokenSource();

            var registry = new Registry(scope.ServiceProvider);
            var hostContext = scope.ServiceProvider.GetRequiredService<HostContext>();
            await using var context = new InitializationContext(scope, hostContext, registry, cts);

            InitializationResult result = new InitializationResult.None();

            if (asyncInitialization is not null)
            {
                await asyncInitialization.Initialize(context, cancellationToken);
                result = new InitializationResult.Completed(metadata, true);
                metadata.Initialized = true;
            }
            else if (syncInitialization is not null)
            {
                syncInitialization!.Initialize(context);
                result = new InitializationResult.Completed(metadata, true);
                metadata.Initialized = true;
            }

            return result;
        }
        catch (Exception e)
        {
            return new InitializationResult.Faulted(e);
        }
    }
}
