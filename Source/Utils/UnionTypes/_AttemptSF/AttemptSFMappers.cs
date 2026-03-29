namespace Utils.UnionTypes;

public static class AttemptSFMappers
{
    public static Attempt<TS2, TF> MapSuccess<TS, TF, TS2>(this Attempt<TS, TF> attempt, Func<TS, TS2> mapper)
    {
        return attempt.Either(success => Attempt<TS2, TF>.Success(mapper(success)), Attempt<TS2, TF>.Failure);
    }
    public static Attempt<TS2, TF> FlatMapSuccess<TS, TF, TS2>(this Attempt<TS, TF> attempt, Func<TS, Attempt<TS2, TF>> mapper)
    {
        return attempt.Either(mapper, Attempt<TS2, TF>.Failure);
    }
    public static Attempt<TS, TF2> MapFailure<TS, TF, TF2>(this Attempt<TS, TF> attempt, Func<TF, TF2> mapper)
    {
        return attempt.Either(Attempt<TS, TF2>.Success, failure => Attempt<TS, TF2>.Failure(mapper(failure)));
    }
    public static Attempt<TS, TF2> FlatMapFailure<TS, TF, TF2>(this Attempt<TS, TF> attempt, Func<TF, Attempt<TS, TF2>> mapper)
    {
        return attempt.Either(Attempt<TS, TF2>.Success, mapper);
    }
}