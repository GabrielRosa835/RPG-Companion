namespace Utils.UnionTypes;

public static class AttemptSGetters
{
    public static TS GetSuccess<TS>(this Attempt<TS> attempt)
    {
        return attempt.Either(success => success, _ => throw failureException());
        InvalidOperationException failureException() => new($"Could not retrieve failure value. Attempt<{typeof(TS).Name}> is a success");
    }
    public static Maybe<TS> GetSuccessOrEmpty<TS>(this Attempt<TS> attempt)
    {
        return attempt.Either(Maybe<TS>.Some, _ => Maybe<TS>.None());
    }
    public static TS GetSuccessOrDefault<TS>(this Attempt<TS> attempt)
    {
        return attempt.Either(success => success, _ => default!);
    }
    public static TS GetSuccessOr<TS>(this Attempt<TS> attempt, TS value)
    {
        return attempt.Either(success => success, _ => value);
    }
    public static TS GetSuccessOr<TS>(this Attempt<TS> attempt, Func<TS> provider)
    {
        return attempt.Either(success => success, _ => provider());
    }
    public static TS GetSuccessOr<TS>(this Attempt<TS> attempt, Func<Exception, TS> recovery)
    {
        return attempt.Either(success => success, recovery);
    }
    public static bool TryGetSuccess<TS>(this Attempt<TS> attempt, out TS value)
    {
        value = attempt.GetSuccessOrDefault();
        return attempt.IsSuccess;
    }
    public static Exception GetFailure<TS>(this Attempt<TS> attempt)
    {
        return attempt.Either(_ => throw successException(), failure => failure);
        InvalidOperationException successException() => new($"Could not retrieve success value. Attempt<{typeof(TS).Name}> is a failure.");
    }
    public static Maybe<Exception> GetFailureOrEmpty<TS>(this Attempt<TS> attempt)
    {
        return attempt.Either(_ => Maybe<Exception>.None(), Maybe<Exception>.Some);
    }
    public static Exception GetFailureOrDefault<TS>(this Attempt<TS> attempt)
    {
        return attempt.Either(_ => default!, failure => failure);
    }
    public static Exception GetFailureOr<TS>(this Attempt<TS> attempt, Exception value)
    {
        return attempt.Either(_ => value, failure => failure);
    }
    public static Exception GetFailureOr<TS>(this Attempt<TS> attempt, Func<Exception> provider)
    {
        return attempt.Either(_ => provider(), failure => failure);
    }
    public static Exception GetFailureOr<TS>(this Attempt<TS> attempt, Func<TS, Exception> enforcer)
    {
        return attempt.Either(enforcer, failure => failure);
    }
    public static bool TryGetFailure<TS>(this Attempt<TS> attempt, out Exception value)
    {
        value = attempt.GetFailureOrDefault();
        return attempt.IsFailure;
    }
}