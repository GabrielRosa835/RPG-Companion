namespace RpgCompanion.Host.Intents;

internal class IntentDispatcher(
    PluginArchives _pluginArchives,
    IntentArchives _intentArchives,
    EnvironmentAccessor _environmentAccessor)
    : IIntentDispatcher
{
    public async Task Dispatch(IIntent intent, CancellationToken cancellationToken = default)
    {
        var (ctx, executor, serviceProvider) = CreateContext(intent, cancellationToken);
        await using (ctx)
        {
            await executor.Execute(serviceProvider, intent, ctx, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<TResult> Dispatch<TResult>(IIntent<TResult> intent, CancellationToken cancellationToken = default)
    {
        var (ctx, executor, serviceProvider) = CreateContext(intent, cancellationToken);
        await using (ctx)
        {
            var result = await executor.Execute(serviceProvider, intent, ctx, cancellationToken).ConfigureAwait(false);
            return (TResult) result!;
        }
    }

    private (IntentContext Context, IntentExecutor Executor, IServiceProvider Services) CreateContext(IIntentBase intent, CancellationToken cancellationToken)
    {
        if (_environmentAccessor.CurrentPlugin is null)
        {
            var descriptor = _intentArchives[intent.GetType()];
            _environmentAccessor.CurrentPlugin = new PluginContext
            {
                Key = descriptor.PluginKey,
            };
        }

        var pluginServices = _pluginArchives[_environmentAccessor.CurrentPlugin.Key].Services;
        var scopeFactory = pluginServices.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateAsyncScope();

        var cts = cancellationToken.CanBeCanceled
            ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
            : new CancellationTokenSource();

        var executor = scope.ServiceProvider.GetRequiredService<IntentExecutor>();
        var ctx = new IntentContext(scope, new Registry(scope.ServiceProvider), cts);

        return (ctx, executor, scope.ServiceProvider);
    }
}
