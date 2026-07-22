namespace RpgCompanion.Core;

public abstract record EventResult
{
    public sealed record None : EventResult;
    public sealed record Halted : EventResult;
    public sealed record Stopped : EventResult;
    public sealed record Completed : EventResult;
    public sealed record Faulted(Exception Exception) : EventResult;
}
