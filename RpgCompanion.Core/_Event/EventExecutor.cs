namespace RpgCompanion.Core;

public abstract record EventExecutor
{
    public sealed record None : EventExecutor;

    public sealed record Sync(EventHandler Handler) : EventExecutor;

    public sealed record Async(EventHandlerAsync Handler) : EventExecutor;

    public abstract record Timed(TimeSpan Interval) : EventExecutor;

    public sealed record TimedSync(EventHandler Handler, TimeSpan Interval) : Timed(Interval);

    public sealed record TimedAsync(EventHandlerAsync Handler, TimeSpan Interval) : Timed(Interval);

    public static implicit operator EventExecutor(EventHandler handler) => new Sync(handler);
    public static implicit operator EventExecutor(EventHandlerAsync handler) => new Async(handler);

    public static implicit operator EventExecutor((EventHandler Handler, TimeSpan Interval) group)
        => new TimedSync(group.Handler, group.Interval);

    public static implicit operator EventExecutor((EventHandlerAsync Handler, TimeSpan Interval) group)
        => new TimedAsync(group.Handler, group.Interval);
}
