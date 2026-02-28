namespace Utils.UnionTypes;

public static class AttemptSFConverters
{
    public static Maybe<TS> ToMaybe<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.GetSuccessOrEmpty();
    }
    public static Attempt<TS> Simplify<TS, TF>(this Attempt<TS, TF> attempt, Func<TF, Exception> mapper)
    {
        return attempt.Either(Attempt<TS>.Success, failure => Attempt<TS>.Failure(mapper(failure)));
    }
    public static Attempt<TS> Simplify<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(Attempt<TS>.Success, _ => Attempt<TS>.Failure(new UnitException()));
    }
    public static Attempt DeepSimplify<TS, TF>(this Attempt<TS, TF> attempt)
    {
        return attempt.Either(_ => Attempt.Success(), _ => Attempt.Failure(new UnitException()));
    }
}