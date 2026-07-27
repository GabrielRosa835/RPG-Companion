namespace RpgCompanion.Core;

internal abstract class IntentExecutor
{
    internal abstract Task<object?> Execute(
        IServiceProvider serviceProvider,
        IIntentBase intent,
        IIntentContext context,
        CancellationToken cancellationToken);

    internal sealed class Sync<TIntent> : IntentExecutor where TIntent : IIntent
    {
        internal override Task<object?> Execute(
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

    internal sealed class Async<TIntent> : IntentExecutor where TIntent : IIntent
    {
        internal override async Task<object?> Execute(
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

    internal sealed class SyncResult<TIntent, TResult> : IntentExecutor where TIntent : IIntent<TResult>
    {
        internal override Task<object?> Execute(
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

    internal sealed class AsyncResult<TIntent, TResult> : IntentExecutor where TIntent : IIntent<TResult>
    {
        internal override async Task<object?> Execute(
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
