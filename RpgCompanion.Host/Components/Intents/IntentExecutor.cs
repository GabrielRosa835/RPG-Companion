namespace RpgCompanion.Host;

internal interface IntentExecutor
{
    Task<object?> Execute(
        IServiceProvider serviceProvider,
        IIntentBase intent,
        IIntentContext context,
        CancellationToken cancellationToken);

    internal readonly struct Sync<TIntent> : IntentExecutor where TIntent : IIntent
    {
        public Task<object?> Execute(
            IServiceProvider serviceProvider,
            IIntentBase intent,
            IIntentContext context,
            CancellationToken cancellationToken)
        {
            var processor = serviceProvider.GetRequiredService<IIntentProcessor<TIntent>>();
            processor.Process((TIntent) intent, context);
            return Task.FromResult<object?>(null);
        }
    }

    internal readonly struct Async<TIntent> : IntentExecutor where TIntent : IIntent
    {
        public async Task<object?> Execute(
            IServiceProvider serviceProvider,
            IIntentBase intent,
            IIntentContext context,
            CancellationToken cancellationToken)
        {
            var processor = serviceProvider.GetRequiredService<IAsyncIntentProcessor<TIntent>>();
            await processor.Process((TIntent) intent, context, cancellationToken);
            return null;
        }
    }

    internal readonly struct SyncResult<TIntent, TResult> : IntentExecutor where TIntent : IIntent<TResult>
    {
        public Task<object?> Execute(
            IServiceProvider serviceProvider,
            IIntentBase intent,
            IIntentContext context,
            CancellationToken cancellationToken)
        {
            var processor = serviceProvider.GetRequiredService<IIntentProcessor<TIntent, TResult>>();
            var result = processor.Process((TIntent) intent, context);
            return Task.FromResult<object?>(result);
        }
    }

    internal readonly struct AsyncResult<TIntent, TResult> : IntentExecutor where TIntent : IIntent<TResult>
    {
        public async Task<object?> Execute(
            IServiceProvider serviceProvider,
            IIntentBase intent,
            IIntentContext context,
            CancellationToken cancellationToken)
        {
            var processor = serviceProvider.GetRequiredService<IAsyncIntentProcessor<TIntent, TResult>>();
            var result = await processor.Process((TIntent) intent, context, cancellationToken);
            return result;
        }
    }
}
