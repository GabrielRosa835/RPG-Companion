namespace RpgCompanion.Host.Configuration;

internal abstract record InitializationExecutor
{
    internal abstract Task Execute(IInitializationContextAsync context);

    internal sealed record Sync(InitializationHandler Handler) : InitializationExecutor
    {
        internal override Task Execute(IInitializationContextAsync context)
        {
            Handler(context);
            return Task.CompletedTask;
        }
    }

    internal sealed record Async(InitializationHandlerAsync Handler) : InitializationExecutor
    {
        internal override Task Execute(IInitializationContextAsync context)
        {
            return Handler(context);
        }
    }
}
