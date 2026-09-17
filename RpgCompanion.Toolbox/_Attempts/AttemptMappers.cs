namespace RpgCompanion.Toolbox;

public static class AttemptMappers
{
    extension(Attempt attempt)
    {
        public Attempt MapFailure<TException>(Func<Exception, TException> mapper) where TException : Exception =>
            attempt.Either(Attempt.Success, failure => Attempt.Failure(mapper(failure)));

        public Attempt FlatMapFailure<TException>(Func<Exception, Attempt> mapper) =>
            attempt.Either(Attempt.Success, mapper);
    }

    extension<TS>(Attempt<TS> attempt)
    {
        public Attempt<TS2> MapSuccess<TS2>(Func<TS, TS2> mapper) =>
            attempt.Either(success => Attempt<TS2>.Success(mapper(success)), Attempt<TS2>.Failure);

        public Attempt<TS2> FlatMapSuccess<TS2>(Func<TS, Attempt<TS2>> mapper) =>
            attempt.Either(mapper, Attempt<TS2>.Failure);

        public Attempt<TS> MapFailure(Func<Exception, Exception> mapper) =>
            attempt.Either(Attempt<TS>.Success, failure => Attempt<TS>.Failure(mapper(failure)));

        public Attempt<TS> FlatMapFailure(Func<Exception, Attempt<TS>> mapper) =>
            attempt.Either(Attempt<TS>.Success, mapper);
    }

    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public Attempt<TS2, TF> MapSuccess<TS2>(Func<TS, TS2> mapper) =>
            attempt.Either(success => Attempt<TS2, TF>.Success(mapper(success)), Attempt<TS2, TF>.Failure);

        public Attempt<TS2, TF> FlatMapSuccess<TS2>(Func<TS, Attempt<TS2, TF>> mapper) =>
            attempt.Either(mapper, Attempt<TS2, TF>.Failure);

        public Attempt<TS, TF2> MapFailure<TF2>(Func<TF, TF2> mapper) =>
            attempt.Either(Attempt<TS, TF2>.Success, failure => Attempt<TS, TF2>.Failure(mapper(failure)));

        public Attempt<TS, TF2> FlatMapFailure<TF2>(Func<TF, Attempt<TS, TF2>> mapper) =>
            attempt.Either(Attempt<TS, TF2>.Success, mapper);
    }
}
