namespace RpgCompanion.Core.Toolbox;

public static class AttemptSMappers
{
    extension<TS>(Attempt<TS> attempt)
    {
        public Attempt<TS2> MapSuccess<TS2>(Func<TS, TS2> mapper)
        {
            return attempt.Either(success => Attempt<TS2>.Success(mapper(success)), Attempt<TS2>.Failure);
        }

        public Attempt<TS2> FlatMapSuccess<TS2>(Func<TS, Attempt<TS2>> mapper)
        {
            return attempt.Either(mapper, Attempt<TS2>.Failure);
        }

        public Attempt<TS> MapFailure(Func<Exception, Exception> mapper)
        {
            return attempt.Either(Attempt<TS>.Success, failure => Attempt<TS>.Failure(mapper(failure)));
        }

        public Attempt<TS> FlatMapFailure<TF2>(Func<Exception, Attempt<TS>> mapper)
        {
            return attempt.Either(Attempt<TS>.Success, mapper);
        }
    }
}
