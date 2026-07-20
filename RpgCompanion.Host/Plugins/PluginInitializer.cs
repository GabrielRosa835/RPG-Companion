namespace RpgCompanion.Host;

using Configuration;

internal class PluginInitializer(
    IServiceScopeFactory _scopeFactory,
    IEnumerable<PluginMetadata> _plugins)
{
    public Task InitializeAll()
    {
        return Task.WhenAll(_plugins.Select(InitializeSingle));
    }

    private async Task InitializeSingle(PluginMetadata metadata)
    {
        Console.WriteLine($"Initializing plugin {metadata.Resource}");
        if (metadata.Initialization is not null)
        {
            var scope = _scopeFactory.CreateAsyncScope();
            var cts = new CancellationTokenSource();
            var context = new InitializationContext(scope, cts)
            {
                Executor = metadata.Initialization!
            };
            await ExecuteInitialization(context);
        }
        metadata.Initialized = true;
    }

    private static async Task ExecuteInitialization(InitializationContext context)
    {
        await using (context)
        {
            await context.Executor.Execute(context);
        }
    }
}
