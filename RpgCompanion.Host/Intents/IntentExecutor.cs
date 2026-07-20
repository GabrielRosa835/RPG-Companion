namespace RpgCompanion.Core;

internal abstract record IntentExecutor
{
    // Used by Dispatch(IIntent)
    internal virtual Task ExecuteAsync(IIntentBase intent, IIntentContextAsync context) =>
        throw new InvalidOperationException("This executor does not support void intents.");

    // Used by Dispatch<TResult>(IIntent<TResult>)
    internal virtual Task<object?> ExecuteWithResultAsync(IIntentBase intent, IIntentContextAsync context) =>
        throw new InvalidOperationException("This executor does not return a result.");

    internal sealed record Sync<TIntent>(IntentHandler<TIntent> Handler) : IntentExecutor where TIntent : IIntent
    {
        internal override Task ExecuteAsync(IIntentBase intent, IIntentContextAsync context)
        {
            Handler((TIntent) intent, context);
            return Task.CompletedTask;
        }
    }

    internal sealed record Async<TIntent>(IntentHandlerAsync<TIntent> Handler) : IntentExecutor where TIntent : IIntent
    {
        internal override Task ExecuteAsync(IIntentBase intent, IIntentContextAsync context)
        {
            return Handler((TIntent) intent, context);
        }
    }

    internal sealed record SyncResult<TIntent, TResult>(IntentHandler<TIntent, TResult> Handler) : IntentExecutor where TIntent : IIntent<TResult>
    {
        internal override Task<object?> ExecuteWithResultAsync(IIntentBase intent, IIntentContextAsync context)
        {
            var result = Handler((TIntent) intent, context);
            return Task.FromResult<object?>(result);
        }
    }

    internal sealed record AsyncResult<TIntent, TResult>(IntentHandlerAsync<TIntent, TResult> Handler) : IntentExecutor where TIntent : IIntent<TResult>
    {
        internal override async Task<object?> ExecuteWithResultAsync(IIntentBase intent, IIntentContextAsync context)
        {
            var result = await Handler((TIntent) intent, context);
            return result;
        }
    }
}
