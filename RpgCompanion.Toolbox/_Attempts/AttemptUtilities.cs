namespace RpgCompanion.Toolbox;

public static class AttemptUtilities
{
    extension(Attempt attempt)
    {
        public Attempt MergeFailure(Attempt other) => other.Either(
            () => attempt,
            otherFailure => attempt.Either(
                () => other,
                failure => new AggregateException(otherFailure, failure)));

        public Attempt MergeSuccess(Attempt other) => other.Either(
            () => attempt.Either(
                () => attempt,
                _ => other),
            failure => attempt);

        public Attempt Or(Attempt other) => attempt.MergeFailure(other);
        public Attempt And(Attempt other) => attempt.MergeSuccess(other);
    }

    extension<TS, TF>(Attempt<TS, TF> attempt)
    {
        public Attempt<TS, TF> IfSuccess(Action<TS> action) => attempt.Either(
            success =>
            {
                action(success);
                return attempt;
            },
            failure => attempt);

        public Attempt<TS, TF> IfFailure(Action<TF> action) => attempt.Either(
            success => attempt,
            failure =>
            {
                action(failure);
                return attempt;
            });

        public Attempt<TS, TF> OnEither(Action<TS> onSuccess, Action<TF> onFailure) => attempt.Either(
            success =>
            {
                onSuccess(success);
                return attempt;
            },
            failure =>
            {
                onFailure(failure);
                return attempt;
            });
    }

    extension<T>(Attempt<T, T> attempt)
    {
        public Attempt<T, T> OnBoth(Action<T> onBoth) => attempt.OnEither(onBoth, onBoth);
    }
}
