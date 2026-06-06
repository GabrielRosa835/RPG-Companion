namespace RpgCompanion.Core.Toolbox;

public static class AttemptSGetters
{
    extension<TS>(Attempt<TS> attempt)
    {
        public TS GetSuccess()
        {
            return attempt.Either(success => success, _ => throw failureException());
            InvalidOperationException failureException() => new($"Could not retrieve failure value. Attempt<{typeof(TS).Name}> is a success");
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

        public TS GetSuccessOr(Func<Exception, TS> recovery)
        {
            return attempt.Either(success => success, recovery);
        }

        public bool TryGetSuccess(out TS value)
        {
            value = attempt.GetSuccessOrDefault();
            return attempt.IsSuccess;
        }

        public Exception GetFailure()
        {
            return attempt.Either(_ => throw successException(), failure => failure);
            InvalidOperationException successException() => new($"Could not retrieve success value. Attempt<{typeof(TS).Name}> is a failure.");
        }

        public Maybe<Exception> GetFailureOrEmpty()
        {
            return attempt.Either(_ => Maybe<Exception>.None(), Maybe<Exception>.Some);
        }

        public Exception GetFailureOrDefault()
        {
            return attempt.Either(_ => default!, failure => failure);
        }

        public Exception GetFailureOr(Exception value)
        {
            return attempt.Either(_ => value, failure => failure);
        }

        public Exception GetFailureOr(Func<Exception> provider)
        {
            return attempt.Either(_ => provider(), failure => failure);
        }

        public Exception GetFailureOr(Func<TS, Exception> enforcer)
        {
            return attempt.Either(enforcer, failure => failure);
        }

        public bool TryGetFailure(out Exception value)
        {
            value = attempt.GetFailureOrDefault();
            return attempt.IsFailure;
        }
    }
}
