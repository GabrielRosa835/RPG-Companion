namespace RpgCompanion.Toolbox;

public static class AttemptConverters
{
    extension<TS>(Attempt<TS> attempt)
    {
        public Maybe<TS> ToMaybe() => attempt.GetSuccessOrEmpty();

        public Attempt Simplify() => attempt.Either(_ => Attempt.Success(), Attempt.Failure);
    }

    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public Maybe<TS> ToMaybe() => attempt.GetSuccessOrEmpty();

        public Attempt<TS> Simplify(Func<TF, Exception> mapper) => attempt.Either(Attempt<TS>.Success, failure => Attempt<TS>.Failure(mapper(failure)));

        public Attempt<TS> Simplify() => attempt.Either(Attempt<TS>.Success, _ => Attempt<TS>.Failure(new EmptyException()));

        public Attempt DeepSimplify() => attempt.Either(_ => Attempt.Success(), _ => Attempt.Failure(new EmptyException()));
    }
}
