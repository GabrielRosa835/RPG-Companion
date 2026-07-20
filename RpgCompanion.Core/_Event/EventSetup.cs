namespace RpgCompanion.Core;

public abstract record EventSetup
{
    public sealed record None : EventSetup;

    public sealed record Sync(EventHandler Handler) : EventSetup;

    public sealed record Async(EventHandlerAsync Handler) : EventSetup;

    public static implicit operator EventSetup(EventHandler handler) => new Sync(handler);
    public static implicit operator EventSetup(EventHandlerAsync handler) => new Async(handler);
}
