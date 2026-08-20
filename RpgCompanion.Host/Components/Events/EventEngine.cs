namespace RpgCompanion.Host;

using System.Collections.Concurrent;

internal class EventEngine(
    EnvironmentAccessor _environmentAccessor,
    PluginAccessor _pluginAccessor)
    : IEventTrigger
{
    private static readonly TimeSpan MinimumExecutionInterval = TimeSpan.FromMilliseconds(10);
    private static readonly TimeSpan FallbackExecutionInterval = TimeSpan.FromMilliseconds(100);

    private readonly ConcurrentDictionary<Guid, EventExecutionContext> _activeContexts = new();

    public async Task<EventResult> Raise(Event e, CancellationToken cancellationToken = default)
    {
        var (currentPlugin, currentContext) = _pluginAccessor.Get(e);
        _environmentAccessor.CurrentPlugin = currentContext;

        var executionContextFactory = currentPlugin.Services.GetRequiredService<EventExecutionContextFactory>();
        var executionContext = executionContextFactory.Create(cancellationToken);

        try
        {
            executionContext.Current = e;
            _activeContexts[executionContext.Id] = executionContext;
            executionContext.Task = StartExecution(executionContext);
            return await executionContext.Task;
        }
        finally
        {
            _activeContexts.TryRemove(executionContext.Id, out _);
        }
    }

    private static async Task<EventResult> StartExecution(EventExecutionContext execution)
    {
        CancellationToken ct = execution.CancellationSource.Token;

        try
        {
            while (!ct.IsCancellationRequested && !execution.Exiting)
            {
                try
                {
                    await execution.Current.Setup(execution.Context, ct);
                }
                catch (OperationCanceledException)
                {
                }

                TimeSpan interval = FallbackExecutionInterval;
                if (execution.Current.ExecutionInterval.HasValue)
                {
                    interval = execution.Current.ExecutionInterval > MinimumExecutionInterval
                        ? execution.Current.ExecutionInterval.Value
                        : MinimumExecutionInterval;
                }

                try
                {
                    while (!ct.IsCancellationRequested && !execution.Continuing)
                    {
                        await execution.Current.Execute(execution.Context, ct);

                        try
                        {
                            await Task.Delay(interval, ct);
                        }
                        catch (OperationCanceledException)
                        {
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                }

                try
                {
                    var safeToken = ct.IsCancellationRequested ? CancellationToken.None : ct;
                    await execution.Current.Teardown(execution.Context, safeToken);
                }
                catch (OperationCanceledException)
                {
                }

                if (execution.Continuing)
                {
                    execution.Current = execution.Next!;
                    execution.Next = null;
                }
            }

            return ct.IsCancellationRequested ? new EventResult.Stopped() : execution.Result!;
        }
        catch (Exception ex)
        {
            return new EventResult.Faulted(ex);
        }
    }
}
