namespace Utils.UnionTypes;

public static class AttemptSFGetters
{
    public static TS GetSuccess<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(success => success, _ => throw failureException());
        InvalidOperationException failureException() => new($"Could not retrieve success value. Attempt<{typeof(TS).Name}, {typeof(TF).Name}> is a failure");
    }
    public static Maybe<TS> GetSuccessOrEmpty<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(Maybe<TS>.Some, _ => Maybe<TS>.None());
    }
    public static TS GetSuccessOrDefault<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(success => success, _ => default!);
    }
    public static TS GetSuccessOr<TS, TF>(this Attempt<TS, TF> attempt, TS value)
    {
        return attempt.Either(success => success, _ => value);
    }
    public static TS GetSuccessOr<TS, TF>(this Attempt<TS, TF> attempt, Func<TS> provider)
    {
        return attempt.Either(success => success, _ => provider());
    }
    public static TS GetSuccessOr<TS, TF>(this Attempt<TS, TF> attempt, Func<TF, TS> recovery)
    {
        return attempt.Either(success => success, recovery);
    }
    public static bool TryGetSuccess<TS, TF>(this Attempt<TS, TF> attempt, out TS value)
    {
        value = attempt.GetSuccessOrDefault();
        return attempt.IsSuccess;
    }
    public static TF GetFailure<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(_ => throw successException(), failure => failure);
        InvalidOperationException successException() => new($"Could not retrieve failure value. Attempt<{typeof(TS).Name}, {typeof(TF).Name}> is a success");
    }
    public static Maybe<TF> GetFailureOrEmpty<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(_ => Maybe<TF>.None(), Maybe<TF>.Some);
    }
    public static TF GetFailureOrDefault<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(_ => default!, failure => failure);
    }
    public static TF GetFailureOr<TS, TF>(this Attempt<TS, TF> attempt, TF value)
    {
        return attempt.Either(_ => value, failure => failure);
    }
    public static TF GetFailureOr<TS, TF>(this Attempt<TS, TF> attempt, Func<TF> provider)
    {
        return attempt.Either(_ => provider(), failure => failure);
    }
    public static TF GetFailureOr<TS, TF>(this Attempt<TS, TF> attempt, Func<TS, TF> enforcer)
    {
        return attempt.Either(enforcer, failure => failure);
    }
    public static bool TryGetFailure<TS, TF>(this Attempt<TS, TF> attempt, out TF value)
    {
        value = attempt.GetFailureOrDefault();
        return attempt.IsFailure;
    }
}