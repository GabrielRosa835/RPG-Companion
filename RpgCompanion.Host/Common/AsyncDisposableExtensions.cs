namespace RpgCompanion.Host.Common;

internal static class AsyncDisposableExtensions
{
    extension(IAsyncDisposable disposable)
    {
        internal ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
            {
                return resourceAsyncDisposable.DisposeAsync();
            }
            resource.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
