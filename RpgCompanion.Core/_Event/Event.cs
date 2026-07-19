namespace RpgCompanion.Core;

public abstract record Event(EventSetup Setup, EventExecutor Execute, EventTeardown Teardown) : EventSetup;

public abstract record EventSetup
{
    public sealed record None : EventSetup;
    public sealed record Sync(EventHandler Handler) : EventSetup;
    public sealed record Async(EventHandlerAsync Handler) : EventSetup;
}

public abstract record EventExecutor
{
    public sealed record None : EventExecutor;
    public sealed record Sync(EventHandler Handler) : EventExecutor;
    public sealed record Async(EventHandlerAsync Handler) : EventExecutor;
    public abstract record Timed(TimeSpan Interval) : EventExecutor;
    public sealed record TimedSync(EventHandler Handler, TimeSpan Interval) : Timed(Interval);
    public sealed record TimedAsync(EventHandlerAsync Handler, TimeSpan Interval) : Timed(Interval);
}

public abstract record EventTeardown
{
    public sealed record None : EventTeardown;
    public sealed record Sync(EventHandler Handler) : EventTeardown;
    public sealed record Async(EventHandlerAsync Handler) : EventTeardown;
}
