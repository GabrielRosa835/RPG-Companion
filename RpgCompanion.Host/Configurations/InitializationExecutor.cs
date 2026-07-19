namespace RpgCompanion.Host.Configuration;

internal abstract record InitializationExecutor
{
    public sealed record Sync(InitializationHandler Handler) : InitializationExecutor;

    public sealed record Async(InitializationAsyncHandler Handler) : InitializationExecutor;
}
