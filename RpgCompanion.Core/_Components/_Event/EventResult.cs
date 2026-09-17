namespace RpgCompanion.Core;

public abstract record EventResult
{
    public sealed record None : EventResult;
    public record Halted : EventResult;
    public record Completed : EventResult;
    public record Completed<TResult>(TResult Result) : Completed;
    public record Faulted(Exception Exception) : EventResult;
}
