namespace RpgCompanion.Core.Toolbox;

public static class AttemptSFGetters
{
    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public TS GetSuccess()
        {
            return attempt.Either(success => success, _ => throw failureException());
            InvalidOperationException failureException() => new($"Could not retrieve success value. Attempt<{typeof(TS).Name}, {typeof(TF).Name}> is a failure");
        }

        public Maybe<TS> GetSuccessOrEmpty()
        {
            return attempt.Either(Maybe<TS>.Some, _ => Maybe<TS>.None());
        }

        public TS GetSuccessOrDefault()
        {
            return attempt.Either(success => success, _ => default!);
        }

        public TS GetSuccessOr(TS value)
        {
            return attempt.Either(success => success, _ => value);
        }

        public TS GetSuccessOr(Func<TS> provider)
        {
            return attempt.Either(success => success, _ => provider());
        }

        public TS GetSuccessOr(Func<TF, TS> recovery)
        {
            return attempt.Either(success => success, recovery);
        }

        public bool TryGetSuccess(out TS value)
        {
            value = attempt.GetSuccessOrDefault();
            return attempt.IsSuccess;
        }

        public TF GetFailure()
        {
            return attempt.Either(_ => throw successException(), failure => failure);
            InvalidOperationException successException() => new($"Could not retrieve failure value. Attempt<{typeof(TS).Name}, {typeof(TF).Name}> is a success");
        }

        public Maybe<TF> GetFailureOrEmpty()
        {
            return attempt.Either(_ => Maybe<TF>.None(), Maybe<TF>.Some);
        }

        public TF GetFailureOrDefault()
        {
            return attempt.Either(_ => default!, failure => failure);
        }

        public TF GetFailureOr(TF value)
        {
            return attempt.Either(_ => value, failure => failure);
        }

        public TF GetFailureOr(Func<TF> provider)
        {
            return attempt.Either(_ => provider(), failure => failure);
        }

        public TF GetFailureOr(Func<TS, TF> enforcer)
        {
            return attempt.Either(enforcer, failure => failure);
        }

        public bool TryGetFailure(out TF value)
        {
            value = attempt.GetFailureOrDefault();
            return attempt.IsFailure;
        }
    }
}
