namespace RpgCompanion.Host.Events;

using System.Collections.Concurrent;
using Core;
using Toolbox;

internal class EventEngine(IServiceScopeFactory _scopeFactory) : ITrigger
{
    private static readonly TimeSpan DefaultSleepTime = TimeSpan.FromMilliseconds(100);

    private readonly ConcurrentDictionary<Guid, EventExecutionContext> _activeContexts = new();

    internal TimeSpan? SleepTime { get; set; }

    internal EventExecutionContext CreateContext(Event first, CancellationToken? ct)
    {
        var scope = _scopeFactory.CreateScope();

        var registry = new Registry(scope.ServiceProvider);
        var storage = new ConcurrentDynamicStorage();
        var eventContext = new EventContextImpl(registry, storage);

        var executionContext = new EventExecutionContext(eventContext)
        {
            Engine = this,
            CurrentEvent = first,
            ServiceScope = scope,
            CancellationSource = ct is not null
                ? CancellationTokenSource.CreateLinkedTokenSource(ct.Value)
                : new CancellationTokenSource(),
        };

        _activeContexts.TryAdd(executionContext.Id, executionContext);
        return executionContext;
    }

    internal void DisposeContext(EventExecutionContext ctx)
    {
        _activeContexts.TryRemove(ctx.Id, out _);
        ctx.Dispose();
    }

    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    public EventTask Raise(Event e, CancellationToken? cancellationToken = null)
    {
        var executionContext = CreateContext(e, cancellationToken);

        executionContext.Task = StartExecution(executionContext)
            .ContinueWith(t =>
            {
                DisposeContext(executionContext);
                return t.Result;
            });

        return new EventTask(executionContext.Task);
    }

    private static async Task<EventResult> StartExecution(EventExecutionContext ctx)
    {
        CancellationToken ct = ctx.CancellationSource.Token;

        try
        {
            while (!ct.IsCancellationRequested && !ctx.Stopping)
            {
                try
                {
                    await ctx.CurrentEvent!.SetupAsync(ctx.Context, ct);
                }
                catch (OperationCanceledException)
                {
                }

                try
                {
                    // The Execute Loop
                    while (!ct.IsCancellationRequested && !ctx.Stopping && ctx.NextEvent is null)
                    {
                        await ctx.CurrentEvent.ExecuteAsync(ctx.Context, ct);

                        TimeSpan sleep = ResolveSleepTime(ctx);
                        if (sleep > TimeSpan.Zero)
                        {
                            await Task.Delay(sleep, ct);
                        }
                        else
                        {
                            await Task.Yield();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Catch cancellation here so we can guarantee Teardown runs
                }

                try
                {
                    await ctx.CurrentEvent.TeardownAsync(ctx.Context, ct);
                }
                catch (OperationCanceledException)
                {
                }

                lock (ctx.Lock)
                {
                    if (ctx.Stopping)
                    {
                        return ctx.Stopping switch
                        {
                            StopRequest.Exiting r => r.Reason,
                            StopRequest.Halting r => r.Reason,
                            StopRequest.Terminating r => r.Reason,
                            _ => new EventResult.Unknown(),
                        };
                    }

                    ctx.CurrentEvent = ctx.NextEvent!;
                    ctx.NextEvent = null;
                }
            }
        }
        catch (Exception ex)
        {
            return new EventResult.Faulted(ex);
        }
        return new EventResult.Completed();
    }

    private static TimeSpan ResolveSleepTime(EventExecutionContext ctx)
    {
        if (ctx.CurrentEvent.SleepTime.HasValue)
            return ctx.CurrentEvent.SleepTime.Value;
        if (ctx.SleepTime.HasValue)
            return ctx.SleepTime.Value;
        if (ctx.Engine.SleepTime.HasValue)
            return ctx.Engine.SleepTime.Value;
        return DefaultSleepTime;
    }
}
