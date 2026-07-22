namespace RpgCompanion.Host.Events;

using System.Collections.Concurrent;
using Core;
using Toolbox;

internal class EventEngine(
    IServiceScopeFactory _scopeFactory,
    IEnvironmentAccessor _environmentAccessor)
    : IEventTrigger
{
    private static readonly TimeSpan MinimumExecutionInterval = TimeSpan.FromMilliseconds(10);
    private static readonly TimeSpan FallbackExecutionInterval = TimeSpan.FromMilliseconds(100);

    private readonly ConcurrentDictionary<Guid, EventExecutionContext> _activeContexts = new();

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    public EventTask Raise(Event e, CancellationToken cancellationToken = default)
    {
        var executionContext = CreateContext(e, cancellationToken);
        executionContext.Task = new EventTask(RunPipelineAsync(executionContext));
        return executionContext.Task;
    }

    private async Task<EventResult> RunPipelineAsync(EventExecutionContext ctx)
    {
        try
        {
            return await StartExecution(ctx);
        }
        finally
        {
            _activeContexts.TryRemove(ctx.Id, out _);
            ctx.Dispose();
        }
    }

    private EventExecutionContext CreateContext(Event first, CancellationToken ct)
    {
        var currentPluginKey = _environmentAccessor.CurrentPlugin?.Key;
        var scope = _scopeFactory.CreateScope();

        var factory = currentPluginKey.HasValue ? null : scope.ServiceProvider.GetKeyedService<IEventFactory>(currentPluginKey);
        factory ??= scope.ServiceProvider.GetRequiredService<DefaultEventFactory>();

        var executionContext = new EventExecutionContext
        {
            Engine = this,
            Current = first,
            ServiceScope = scope,
            CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(ct),
            Factory = factory,
            Registry = new Registry(scope.ServiceProvider),
            Storage = new ConcurrentDynamicStorage(),
        };

        _activeContexts.TryAdd(executionContext.Id, executionContext);
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
