namespace RpgCompanion.Core.Toolbox;

public static class AttemptSConverters
{
    extension<TS>(Attempt<TS> attempt)
    {
        public Maybe<TS> ToMaybe()
        {
            return attempt.GetSuccessOrEmpty();
        }

        public Attempt Simplify()
        {
            return attempt.Either(_ => Attempt.Success(), Attempt.Failure);
        }
    }
}
