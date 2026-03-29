namespace Utils.UnionTypes;

public static class MaybeAsync
{
    public static Task<TR> Either<T, TR>(
        this Task<Maybe<T>> attemptTask, Func<T, TR> onSome, Func<TR> onEmpty, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.Either(onSome, onEmpty), cancellationToken);
    }
}