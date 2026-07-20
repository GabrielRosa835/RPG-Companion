namespace RpgCompanion.Host.Intents;

public class IntentDispatcher(
    IServiceScopeFactory _scopeFactory)
    : IIntentDispatcher
{
    public IntentTask Dispatch(IIntent intent, CancellationToken cancellationToken = default)
    {
        var ctx = CreateContext(intent, cancellationToken);
        return new IntentTask(ExecuteAsync(intent, ctx));
    }

    public IntentTask<TResult> Dispatch<TResult>(IIntent<TResult> intent, CancellationToken cancellationToken = default)
    {
        var ctx = CreateContext(intent, cancellationToken);
        return new IntentTask<TResult>(ExecuteAndCastAsync(intent, ctx));
    }

    private IntentContext CreateContext(IIntentBase intent, CancellationToken cancellationToken)
    {
        var scope = _scopeFactory.CreateAsyncScope();
        var intentType = intent.GetType();
        var executor = scope.ServiceProvider.GetRequiredKeyedService<IntentExecutor>(intentType);

        var cts = cancellationToken.CanBeCanceled
            ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
            : new CancellationTokenSource();

        return new IntentContext(scope, new Registry(scope.ServiceProvider), cts)
        {
            Executor = executor,
        };
    }

    private static async Task ExecuteAsync(IIntent intent, IntentContext ctx)
    {
        await using (ctx)
        {
            await ctx.Executor.ExecuteAsync(intent, ctx).ConfigureAwait(false);
        }
    }

    private static async Task<TResult> ExecuteAndCastAsync<TResult>(IIntent<TResult> intent, IntentContext ctx)
    {
        await using (ctx)
        {
            var result = await ctx.Executor.ExecuteWithResultAsync(intent, ctx).ConfigureAwait(false);
            return (TResult) result!;
        }
    }
}
