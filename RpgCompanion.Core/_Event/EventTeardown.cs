namespace RpgCompanion.Core;

public abstract record EventTeardown
{
    public sealed record None : EventTeardown;

    public sealed record Sync(EventHandler Handler) : EventTeardown;

    public sealed record Async(EventHandlerAsync Handler) : EventTeardown;

    public static implicit operator EventTeardown(EventHandler handler) => new Sync(handler);
    public static implicit operator EventTeardown(EventHandlerAsync handler) => new Async(handler);
}
