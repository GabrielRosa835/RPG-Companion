namespace Utils.UnionTypes;

public static class AttemptSConverters
{
    public static Maybe<TS> ToMaybe<TS>(this Attempt<TS> attempt)
    {
        return attempt.GetSuccessOrEmpty();
    }
    public static Attempt Simplify<TS>(this Attempt<TS> attempt)
    {
        return attempt.Either(_ => Attempt.Success(), Attempt.Failure);
    }
}