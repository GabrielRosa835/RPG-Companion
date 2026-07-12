namespace RpgCompanion.Core;

public abstract record EventResult
{
    public sealed record Unknown : EventResult;
    public sealed record Completed : EventResult;
    public sealed record Faulted(Exception ex) : EventResult;
}
