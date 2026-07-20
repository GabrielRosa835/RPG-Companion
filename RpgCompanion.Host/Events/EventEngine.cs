namespace RpgCompanion.Host.Events;

using System.Collections.Concurrent;
using Core;
using Toolbox;

internal class EventEngine(
    IServiceScopeFactory _scopeFactory,
    IEnvironmentAccessor _environmentAccessor)
    : IEventTrigger
{
    private static readonly TimeSpan FallbackSleepTime = TimeSpan.FromMilliseconds(100);

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

    private async Task RunPipelineAsync(EventExecutionContext ctx)
    {
        try
        {
            var result = await StartExecution(ctx);
            ctx.Task.Result = result;
        }
        finally
        {
            _activeContexts.TryRemove(ctx.Id, out _);
            ctx.Dispose();
        }
    }

    private EventExecutionContext CreateContext(Event first, CancellationToken? ct)
    {
        var scope = _scopeFactory.CreateScope();

        var registry = new Registry(scope.ServiceProvider);
        var storage = new ConcurrentDynamicStorage();
        var eventContext = new EventContext(registry, storage);

        var executionContext = new EventExecutionContext(eventContext)
        {
            Engine = this,
            Current = first,
            ServiceScope = scope,
            CancellationSource = ct is not null
                ? CancellationTokenSource.CreateLinkedTokenSource(ct.Value)
                : new CancellationTokenSource(),
        };

        _activeContexts.TryAdd(executionContext.Id, executionContext);
        return executionContext;
    }

    private static async Task<EventResult> StartExecution(EventExecutionContext ctx)
    {
        CancellationToken ct = ctx.CancellationSource.Token;

        try
        {
            while (!ct.IsCancellationRequested)
            {
                // Track the primary directive for this specific Event
                EventResult directive = new EventResult.None();

                try
                {
                    // 1. SETUP PHASE
                    directive = ctx.Current.EventSetup switch
                    {
                        EventSetup.Sync s => s.Handler(ctx.Context),
                        EventSetup.Async s => await s.Handler(ctx.Context),
                        _ => new EventResult.None(),
                    };

                    // 2. EXECUTE PHASE (Skip if Setup told us to Stop/Fault)
                    if (directive is not EventResult.Stopped and not EventResult.Faulted)
                    {
                        TimeSpan interval = ctx.Current.EventExecutor is EventExecutor.Timed t && t.Interval > TimeSpan.Zero
                            ? t.Interval
                            : FallbackSleepTime;

                        bool isRepeating = true;
                        while (isRepeating && !ct.IsCancellationRequested)
                        {
                            var execResult = ctx.Current.EventExecutor switch
                            {
                                EventExecutor.Sync s => s.Handler(ctx.Context),
                                EventExecutor.Async s => await s.Handler(ctx.Context),
                                EventExecutor.TimedSync s => s.Handler(ctx.Context),
                                EventExecutor.TimedAsync s => await s.Handler(ctx.Context),
                                _ => new EventResult.None(),
                            };

                            if (execResult is EventResult.Repeat)
                            {
                                await Task.Delay(interval, ct);
                            }
                            else
                            {
                                // Capture the final instruction from Execute (e.g., Continue, Stop, Completed)
                                directive = execResult;
                                isRepeating = false;
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    directive = new EventResult.Stopped();
                }
                catch (Exception ex)
                {
                    directive = new EventResult.Faulted(ex);
                }

                // 3. TEARDOWN PHASE
                try
                {
                    var teardownResult = ctx.Current.EventTeardown switch
                    {
                        EventTeardown.Sync s => s.Handler(ctx.Context),
                        EventTeardown.Async s => await s.Handler(ctx.Context),
                        _ => new EventResult.None(),
                    };

                    // If Teardown returns a critical directive (Fault or Continue), let it override.
                    // Otherwise, preserve the directive from Setup/Execute.
                    if (teardownResult is EventResult.Faulted or EventResult.Continue)
                    {
                        directive = teardownResult;
                    }
                }
                catch (Exception ex)
                {
                    directive = new EventResult.Faulted(ex);
                }

                // 4. EVALUATE PIPELINE TRANSITION
                if (directive is EventResult.Continue c && !ct.IsCancellationRequested)
                {
                    // No lock needed here unless ctx.Current is actively being mutated by external threads,
                    // which breaks the single-responsibility pipeline concept anyway.
                    ctx.Current = c.NextEvent;
                }
                else
                {
                    // Break the infinite loop! Return whatever state we ended on.
                    return directive is EventResult.None ? new EventResult.Completed() : directive;
                }
            }

            return new EventResult.Stopped(); // Reached if the while loop is broken by cancellation
        }
        catch (Exception ex)
        {
            return new EventResult.Faulted(ex);
        }
    }
}
