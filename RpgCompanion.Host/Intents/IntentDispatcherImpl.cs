namespace RpgCompanion.Host.Intents;

public class IntentDispatcherImpl(
    IServiceProvider _serviceProvider,
    IServiceScopeFactory _scopeFactory)
    : IIntentDispatcher
{
    public IntentTask Dispatch(IIntent intent, CancellationToken cancellationToken = default)
    {
        var intentType = intent.GetType();
        var executor = _serviceProvider.GetRequiredKeyedService<IntentExecutor>(intentType);

        var scope = _scopeFactory.CreateAsyncScope();
        var context = new IntentContextImpl(scope, new Registry(scope.ServiceProvider));

        return new IntentTask(ExecuteAsync());

        async Task ExecuteAsync()
        {
            await using (scope)
            {
                await executor.ExecuteAsync(intent, context, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public IntentTask<TResult> Dispatch<TResult>(IIntent<TResult> intent, CancellationToken cancellationToken = default)
    {
        var intentType = intent.GetType();
        var executor = _serviceProvider.GetRequiredKeyedService<IntentExecutor>(intentType);

        var scope = _scopeFactory.CreateAsyncScope();
        var context = new IntentContextImpl(scope, new Registry(scope.ServiceProvider));

        return new IntentTask<TResult>(ExecuteAndCastAsync());

        async Task<TResult> ExecuteAndCastAsync()
        {
            await using (scope)
            {
                var result = await executor.ExecuteWithResultAsync(intent, context, cancellationToken).ConfigureAwait(false);
                return (TResult)result!;
            }
        }
    }
}
