namespace RpgCompanion.Toolbox;

public static class AttemptSFUtilities
{
    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public Attempt<TS, TF> IfSuccess(Action<TS> action)
        {
            return attempt.Either(success =>
            {
                action(success);
                return attempt;
            }, _ => attempt);
        }

        public Attempt<TS, TF> IfFailure(Action<TF> action)
        {
            return attempt.Either(_ => attempt, failure =>
            {
                action(failure);
                return attempt;
            });
        }

        public Attempt<TS, TF> OnEither(Action<TS> onSuccess, Action<TF> onFailure)
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
    }

    extension<T>(Attempt<T, T> attempt)
    {
        public Attempt<T, T> OnBoth(Action<T> onBoth)
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
}
