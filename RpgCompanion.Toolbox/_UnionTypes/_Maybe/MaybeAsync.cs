namespace RpgCompanion.Toolbox;

public static class MaybeAsync
{
    extension<T>(Task<Maybe<T>> attemptTask)
    {
        public Task<TR> Either<TR>(Func<T, TR> onSome, Func<TR> onEmpty, CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.Either(onSome, onEmpty), cancellationToken);
        }
    }
}
