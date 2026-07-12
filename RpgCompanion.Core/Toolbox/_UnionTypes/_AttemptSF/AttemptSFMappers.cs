namespace RpgCompanion.Toolbox;

public static class AttemptSFMappers
{
    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public Attempt<TS2, TF> MapSuccess<TS2>(Func<TS, TS2> mapper)
        {
            return attempt.Either(success => Attempt<TS2, TF>.Success(mapper(success)), Attempt<TS2, TF>.Failure);
        }

        public Attempt<TS2, TF> FlatMapSuccess<TS2>(Func<TS, Attempt<TS2, TF>> mapper)
        {
            return attempt.Either(mapper, Attempt<TS2, TF>.Failure);
        }

        public Attempt<TS, TF2> MapFailure<TF2>(Func<TF, TF2> mapper)
        {
            return attempt.Either(Attempt<TS, TF2>.Success, failure => Attempt<TS, TF2>.Failure(mapper(failure)));
        }

        public Attempt<TS, TF2> FlatMapFailure<TF2>(Func<TF, Attempt<TS, TF2>> mapper)
        {
            return attempt.Either(Attempt<TS, TF2>.Success, mapper);
        }
    }
}
