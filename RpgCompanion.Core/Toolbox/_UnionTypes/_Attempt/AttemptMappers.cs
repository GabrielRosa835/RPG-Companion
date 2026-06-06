namespace RpgCompanion.Core.Toolbox;

public static class AttemptMappers
{
    extension(Attempt attempt)
    {
        public Attempt MapFailure<TException>(Func<Exception, TException> mapper) where TException : Exception
        {
            return attempt.Either(Attempt.Success, failure => Attempt.Failure(mapper(failure)));
        }

        public Attempt MapFailure<TException>(Func<Exception, Attempt> mapper)
        {
            return attempt.Either(Attempt.Success, mapper);
        }
    }
}
