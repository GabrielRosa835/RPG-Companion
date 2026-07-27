namespace RpgCompanion.Host.Events;

using System.Collections.Concurrent;
using Core;
using Toolbox;

internal class EventEngine(
    EventArchives _eventArchives,
    PluginArchives _pluginArchives,
    EnvironmentAccessor _environmentAccessor)
    : IEventTrigger
{
    private static readonly TimeSpan MinimumExecutionInterval = TimeSpan.FromMilliseconds(10);
    private static readonly TimeSpan FallbackExecutionInterval = TimeSpan.FromMilliseconds(100);

    private readonly ConcurrentDictionary<Guid, EventExecutionContext> _activeContexts = new();

    public async Task<EventResult> Raise(Event e, CancellationToken cancellationToken = default)
    {
        await using var executionContext = CreateContext(e, cancellationToken);
        try
        {
            return await StartExecution(executionContext);
        }
        finally
        {
            _activeContexts.TryRemove(executionContext.Id, out _);
        }
    }

    private EventExecutionContext CreateContext(Event e, CancellationToken cancellationToken)
    {
        if (_environmentAccessor.CurrentPlugin is null)
        {
            var descriptor = _eventArchives[e.GetType()];
            _environmentAccessor.CurrentPlugin = new PluginContext
            {
                Key = descriptor.PluginKey,
            };
        }

        var pluginServices = _pluginArchives[_environmentAccessor.CurrentPlugin.Key].Services;
        var scopeFactory = pluginServices.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateAsyncScope();

        var factory = scope.ServiceProvider.GetService<IEventFactory>();
        factory ??= scope.ServiceProvider.GetRequiredService<DefaultEventFactory>();

        var cts = cancellationToken.CanBeCanceled
            ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
            : new CancellationTokenSource();

        var executionContext = new EventExecutionContext
        {
            Engine = this,
            Current = e,
            ServiceScope = scope,
            CancellationSource = cts,
            Factory = factory,
            Registry = new Registry(scope.ServiceProvider),
            Storage = new ConcurrentDynamicStorage(),
        };

        _activeContexts[executionContext.Id] = executionContext;

        return executionContext;
    }

    private static async Task<EventResult> StartExecution(EventExecutionContext ctx)
    {
        CancellationToken ct = ctx.CancellationSource.Token;

        try
        {
            while (!ct.IsCancellationRequested && !ctx.Exiting)
            {
                try
                {
                    await ctx.Current.Setup(ctx, ct);
                }
                catch (OperationCanceledException) {}

                TimeSpan interval = FallbackExecutionInterval;
                if (ctx.Current.ExecutionInterval.HasValue)
                {
                    interval = ctx.Current.ExecutionInterval > MinimumExecutionInterval
                        ? ctx.Current.ExecutionInterval.Value
                        : MinimumExecutionInterval;
                }

                try
                {
                    while (!ct.IsCancellationRequested && !ctx.Continuing)
                    {
                        await ctx.Current.Execute(ctx, ct);

                        try
                        {
                            await Task.Delay(interval, ct);
                        }
                        catch (OperationCanceledException) {}
                    }
                }
                catch (OperationCanceledException) {}

                try
                {
                    var safeToken = ct.IsCancellationRequested ? CancellationToken.None : ct;
                    await ctx.Current.Teardown(ctx, safeToken);
                }
                catch (OperationCanceledException) {}

                if (ctx.Continuing)
                {
                    ctx.Current = ctx.Next!;
                    ctx.Next = null;
                }
            }

            return ct.IsCancellationRequested ? new EventResult.Stopped() : ctx.Result!;
        }
        catch (Exception ex)
        {
            return new EventResult.Faulted(ex);
        }
    }
}
