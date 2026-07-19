namespace RpgCompanion.Core;

public abstract record EventResult
{
    public sealed record None : EventResult;
    public sealed record Completed : EventResult;
    public sealed record Stopped : EventResult;
    public sealed record Faulted(Exception Exception) : EventResult;
    public sealed record Continue(Event NextEvent) : EventResult;
    public sealed record Repeat : EventResult;
}
