namespace Utils.UnionTypes;

public static class AttemptSFUtilities
{
    public static Attempt<TS, TF> IfSuccess<TS, TF>(this Attempt<TS, TF> attempt, Action<TS> action)
    {
        return attempt.Either(success =>
        {
            action(success);
            return attempt;
        }, _ => attempt);
    }
    public static Attempt<TS, TF> IfFailure<TS, TF>(this Attempt<TS, TF> attempt, Action<TF> action)
    {
        return attempt.Either(_ => attempt, failure =>
        {
            action(failure);
            return attempt;
        });
    }
    public static Attempt<TS, TF> OnEither<TS, TF>(this Attempt<TS, TF> attempt, Action<TS> onSuccess, Action<TF> onFailure)
    {
        if (attempt.TryGetSuccess(out var successValue))
        {
            onSuccess(successValue);
        }
        else if (attempt.TryGetFailure(out var failureValue))
        {
            onFailure(failureValue);
        }
        return attempt;
    }

    public static Attempt<T, T> OnBoth<T>(this Attempt<T, T> attempt, Action<T> onBoth)
    {
        if (attempt.TryGetSuccess(out var successValue))
        {
            onBoth(successValue);
        }
        else if (attempt.TryGetFailure(out var failureValue))
        {
            onBoth(failureValue);
        }
        return attempt;
    }
}