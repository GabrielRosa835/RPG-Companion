namespace Utils.UnionTypes;

public static class AttemptSFAsync
{
    public static Task<TR> Either<TS, TF, TR>(
        this Task<Attempt<TS, TF>> attemptTask, Func<TS, TR> onSuccess, Func<TF, TR> onFailure, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.Either(onSuccess, onFailure), cancellationToken);
    }
    public static Task<Attempt<TS>> Simplify<TS, TF>(
        this Task<Attempt<TS, TF>> attemptTask, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.Simplify(), cancellationToken);
    }
}