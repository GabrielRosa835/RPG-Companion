namespace Utils.UnionTypes;

public static class AttemptGetters
{
    public static Exception GetFailure(this Attempt attempt)
    {
        return attempt.Either(() => throw successException(), failure => failure);
        
        InvalidOperationException successException() => new("Cannot retrieve failure value. Attempt is a success");
    }
    public static Maybe<Exception> GetFailureOrEmpty(this Attempt attempt)
    {
        return attempt.Either(Maybe<Exception>.None, Maybe<Exception>.Some);
    }
    public static Exception GetFailureOrDefault(this Attempt attempt)
    {
        return attempt.Either(() => default!, failure => failure);
    }
    public static Exception GetFailureOr(this Attempt attempt, Exception value)
    {
        return attempt.Either(() => value, failure => failure);
    }
    public static bool TryGetFailure(this Attempt attempt, out Exception value)
    {
        value = attempt.GetFailureOrDefault();
        return attempt.IsFailure;
    }
}