namespace RpgCompanion.Host;

internal class IntentDispatcher(
    IntentArchives _intentArchives,
    EnvironmentAccessor _environmentAccessor,
    PluginAccessor _pluginAccessor)
    : IIntentDispatcher
{
    public async Task Dispatch(IIntent intent, CancellationToken cancellationToken = default)
    {
        var (currentPlugin, currentContext) = _pluginAccessor.Get(intent);
        _environmentAccessor.CurrentPlugin = currentContext;

        var ctx = currentPlugin.Services.GetRequiredService<IntentContext>();
        ctx.CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        IntentExecutor executor = _intentArchives.Executors[intent.GetType()];

        await using (ctx)
        {
            await executor.Execute(ctx.Scope.ServiceProvider, intent, ctx, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<TResult> Dispatch<TResult>(IIntent<TResult> intent, CancellationToken cancellationToken = default)
    {
        var (currentPlugin, currentContext) = _pluginAccessor.Get(intent);
        _environmentAccessor.CurrentPlugin = currentContext;

        var ctx = currentPlugin.Services.GetRequiredService<IntentContext>();
        ctx.CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        IntentExecutor executor = _intentArchives.Executors[intent.GetType()];

        await using (ctx)
        {
            var result = await executor.Execute(ctx.Scope.ServiceProvider, intent, ctx, cancellationToken).ConfigureAwait(false);
            return (TResult) result!;
        }
    }
}
