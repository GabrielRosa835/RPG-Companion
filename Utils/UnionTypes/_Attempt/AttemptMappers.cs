namespace Utils.UnionTypes;

public static class AttemptMappers
{
    public static Attempt MapFailure<TException>(this Attempt attempt, Func<Exception, TException> mapper) where TException : Exception
    {
        return attempt.Either(Attempt.Success, failure => Attempt.Failure(mapper(failure)));
    }
    public static Attempt MapFailure<TException>(this Attempt attempt, Func<Exception, Attempt> mapper)
    {
        return attempt.Either(Attempt.Success, mapper);
    }
}