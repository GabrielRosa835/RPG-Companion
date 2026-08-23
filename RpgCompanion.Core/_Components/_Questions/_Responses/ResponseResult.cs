namespace RpgCompanion.Core;

public abstract record ResponseResult
{
    public record None : ResponseResult;
    public record TimedOut : ResponseResult;
    public record Faulted(Exception Exception) : ResponseResult;
    public record Completed<TResponse>(TResponse Response) : ResponseResult;
    public record UnacceptableSchema<TSchema, TPayload>(TSchema Schema) : ResponseResult where TSchema : IResponseSchema<TPayload>;
}
