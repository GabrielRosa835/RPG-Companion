namespace RpgCompanion.Core.Toolbox;

public static class AttemptSFConverters
{
    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public Maybe<TS> ToMaybe()
        {
            return attempt.GetSuccessOrEmpty();
        }

        public Attempt<TS> Simplify(Func<TF, Exception> mapper)
        {
            return attempt.Either(Attempt<TS>.Success, failure => Attempt<TS>.Failure(mapper(failure)));
        }

        public Attempt<TS> Simplify()
        {
            return attempt.Either(Attempt<TS>.Success, _ => Attempt<TS>.Failure(new NoneException()));
        }

        public Attempt DeepSimplify()
        {
            return attempt.Either(_ => Attempt.Success(), _ => Attempt.Failure(new NoneException()));
        }
    }
}
