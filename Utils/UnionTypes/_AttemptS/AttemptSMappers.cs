namespace Utils.UnionTypes;

public static class AttemptSMappers
{
    public static Attempt<TS2> MapSuccess<TS, TS2>(this Attempt<TS> attempt, Func<TS, TS2> mapper)
    {
        return attempt.Either(success => Attempt<TS2>.Success(mapper(success)), Attempt<TS2>.Failure);
    }
    public static Attempt<TS2> FlatMapSuccess<TS, TS2>(this Attempt<TS> attempt, Func<TS, Attempt<TS2>> mapper)
    {
        return attempt.Either(mapper, Attempt<TS2>.Failure);
    }
    public static Attempt<TS> MapFailure<TS>(this Attempt<TS> attempt, Func<Exception, Exception> mapper)
    {
        return attempt.Either(Attempt<TS>.Success, failure => Attempt<TS>.Failure(mapper(failure)));
    }
    public static Attempt<TS> FlatMapFailure<TS, TF2>(this Attempt<TS> attempt, Func<Exception, Attempt<TS>> mapper)
    {
        return attempt.Either(Attempt<TS>.Success, mapper);
    }
}