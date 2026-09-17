namespace RpgCompanion.Host;

internal interface IntentExecutor
{
    Task<object?> Execute(
        object intent,
        Func<Type, object> services,
        IntentContext context);

    internal readonly struct Sync<TIntent> : IntentExecutor where TIntent : IIntent
    {
        public Task<object?> Execute(
            object intent,
            Func<Type, object> services,
            IntentContext context)
        {
            var handler = (IIntentHandler<TIntent>) services(typeof(IIntentHandler<TIntent>));
            handler.Handle((TIntent) intent, context);
            return Task.FromResult<object?>(null);
        }
    }

    internal readonly struct Async<TIntent> : IntentExecutor where TIntent : IIntent
    {
        public async Task<object?> Execute(
            object intent,
            Func<Type, object> services,
            IntentContext context)
        {
            var handler = (IIntentHandler<TIntent>) services(typeof(IIntentHandler<TIntent>));
            await handler.Handle((TIntent) intent, context);
            return null;
        }
    }

    internal readonly struct SyncResult<TIntent, TResult> : IntentExecutor where TIntent : IIntent<TResult>
    {
        public Task<object?> Execute(
            object intent,
            Func<Type, object> services,
            IntentContext context)
        {
            var handler = (IIntentHandler<TIntent, TResult>) services(typeof(IIntentHandler<TIntent, TResult>));
            var result = handler.Handle((TIntent) intent, context);
            return Task.FromResult<object?>(result);
        }
    }

    internal readonly struct AsyncResult<TIntent, TResult> : IntentExecutor where TIntent : IIntent<TResult>
    {
        public async Task<object?> Execute(
            object intent,
            Func<Type, object> services,
            IntentContext context)
        {
            var handler = (IIntentHandler<TIntent, TResult>) services(typeof(IIntentHandler<TIntent, TResult>));
            var result = await handler.Handle((TIntent) intent, context);
            return result;
        }
    }
}
