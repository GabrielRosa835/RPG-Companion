namespace RpgCompanion.Core.Toolbox;

public static class AttemptGetters
{
    extension(Attempt attempt)
    {
        public Exception GetFailure()
        {
            return attempt.Either(() => throw successException(), failure => failure);

            InvalidOperationException successException() => new("Cannot retrieve failure value. Attempt is a success");
        }

        public Maybe<Exception> GetFailureOrEmpty()
        {
            return attempt.Either(Maybe<Exception>.None, Maybe<Exception>.Some);
        }

        public Exception GetFailureOrDefault()
        {
            return attempt.Either(() => default!, failure => failure);
        }

        public Exception GetFailureOr(Exception value)
        {
            return attempt.Either(() => value, failure => failure);
        }

        public bool TryGetFailure(out Exception value)
        {
            value = attempt.GetFailureOrDefault();
            return attempt.IsFailure;
        }
    }
}
