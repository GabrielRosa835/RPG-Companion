namespace RpgCompanion.Host;

internal class PluginInitializer : IPluginInitializer
{
    public async Task<List<InitializationResult>> InitializeMany(IEnumerable<LoadedPluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        var initializationTasks = plugins.Select(p => InitializeSingle(p, cancellationToken)).ToList();
        await Task.WhenAll(initializationTasks);
        return initializationTasks.Select(t => t.Result).ToList();
    }

    public async Task<InitializationResult> InitializeSingle(LoadedPluginMetadata metadata, CancellationToken cancellationToken = default)
    {
        try
        {
            var scopeProvider = metadata.Services.GetRequiredService<ScopeProvider>();
            var scope = scopeProvider.CreateScope();
            var initializationContextFactory = scope.ServiceProvider.GetRequiredService<InitializationContextFactory>();

            await using var context = initializationContextFactory.Create(cancellationToken);

            var executor = new InitializationExecutor(scope.ServiceProvider);
            var result = await executor.Initialize(context, cancellationToken);

            var initializedMetadata = InitializedPluginMetadata.Create(metadata);

            return new InitializationResult.Completed(initializedMetadata, result);
        }
        catch (Exception e)
        {
            return new InitializationResult.Faulted(e);
        }
    }
}
