namespace RpgCompanion.Toolbox;

public static class AttemptGetters
{
    extension(Attempt attempt)
    {
        public Exception GetFailure() => attempt.Either(() => throw Attempt.SuccessException, failure => failure);

        public Maybe<Exception> GetFailureOrEmpty() => attempt.Either(Maybe<Exception>.None, Maybe<Exception>.Some);

        public Exception GetFailureOrDefault() => attempt.Either(() => default!, failure => failure);

        public Exception GetFailureOr(Exception value) => attempt.Either(() => value, failure => failure);

        public bool IsFailure(out Exception value)
        {
            value = attempt.GetFailureOrDefault();
            return attempt.IsFailure;
        }

        private static InvalidOperationException SuccessException =>
            new("Cannot retrieve failure value. Attempt is a success");
    }

    extension<TS>(Attempt<TS> attempt)
    {
        public TS GetSuccess() => attempt.Either(success => success, _ => throw Attempt<TS>.FailureException);

        public Maybe<TS> GetSuccessOrEmpty() => attempt.Either(Maybe<TS>.Some, _ => Maybe<TS>.None());

        public TS GetSuccessOrDefault() => attempt.Either(success => success, _ => default!);

        public TS GetSuccessOr(TS value) => attempt.Either(success => success, _ => value);

        public TS GetSuccessOr(Func<TS> provider) => attempt.Either(success => success, _ => provider());

        public TS GetSuccessOr(Func<Exception, TS> recovery) => attempt.Either(success => success, recovery);

        public bool IsSuccess(out TS value)
        {
            value = attempt.GetSuccessOrDefault();
            return attempt.IsSuccess;
        }

        public Exception GetFailure() => attempt.Either(_ => throw Attempt<TS>.SuccessException, failure => failure);

        public Maybe<Exception> GetFailureOrEmpty() => attempt.Either(_ => Maybe<Exception>.None(), Maybe<Exception>.Some);

        public Exception GetFailureOrDefault() => attempt.Either(_ => default!, failure => failure);

        public Exception GetFailureOr(Exception value) => attempt.Either(_ => value, failure => failure);

        public Exception GetFailureOr(Func<Exception> provider) => attempt.Either(_ => provider(), failure => failure);

        public Exception GetFailureOr(Func<TS, Exception> enforcer) => attempt.Either(enforcer, failure => failure);

        public bool IsFailure(out Exception value)
        {
            value = attempt.GetFailureOrDefault();
            return attempt.IsFailure;
        }

        private static InvalidOperationException FailureException =>
            new($"Could not retrieve failure value. Attempt<{typeof(TS).Name}> is a success");

        private static InvalidOperationException SuccessException =>
            new($"Could not retrieve success value. Attempt<{typeof(TS).Name}> is a failure.");
    }

    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public TS GetSuccess() => attempt.Either(success => success, _ => throw Attempt<TS, TF>.FailureException);

        public Maybe<TS> GetSuccessOrEmpty() => attempt.Either(Maybe<TS>.Some, _ => Maybe<TS>.None());

        public TS GetSuccessOrDefault() => attempt.Either(success => success, _ => default!);

        public TS GetSuccessOr(TS value) => attempt.Either(success => success, _ => value);

        public TS GetSuccessOr(Func<TS> provider) => attempt.Either(success => success, _ => provider());

        public TS GetSuccessOr(Func<TF, TS> recovery) => attempt.Either(success => success, recovery);

        public bool IsSuccess(out TS value)
        {
            value = attempt.GetSuccessOrDefault();
            return attempt.IsSuccess;
        }

        public TF GetFailure() => attempt.Either(_ => throw Attempt<TS, TF>.SuccessException, failure => failure);

        public Maybe<TF> GetFailureOrEmpty() => attempt.Either(_ => Maybe<TF>.None(), Maybe<TF>.Some);

        public TF GetFailureOrDefault() => attempt.Either(_ => default!, failure => failure);

        public TF GetFailureOr(TF value) => attempt.Either(_ => value, failure => failure);

        public TF GetFailureOr(Func<TF> provider) => attempt.Either(_ => provider(), failure => failure);

        public TF GetFailureOr(Func<TS, TF> enforcer) => attempt.Either(enforcer, failure => failure);

        public bool IsFailure(out TF failureValue)
        {
            failureValue = attempt.GetFailureOrDefault();
            return attempt.IsFailure;
        }

        public bool IsFailure(out TF failureValue, out TS successValue)
        {
            failureValue = attempt.GetFailureOrDefault();
            successValue = attempt.GetSuccessOrDefault();
            return attempt.IsFailure;
        }

        private static InvalidOperationException FailureException =>
            new($"Could not retrieve success value. Attempt<{typeof(TS).Name}, {typeof(TF).Name}> is a failure");

        private static InvalidOperationException SuccessException =>
            new($"Could not retrieve failure value. Attempt<{typeof(TS).Name}, {typeof(TF).Name}> is a success");
    }
}
