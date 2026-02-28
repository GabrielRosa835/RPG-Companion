namespace Utils.UnionTypes;

public static class AttemptSAsync
{
    public static Task<TR> Either<TS, TR>(
        this Task<Attempt<TS>> attemptTask, Func<TS, TR> onSuccess, Func<Exception, TR> onFailure, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.Either(onSuccess, onFailure), cancellationToken);
    }
    public static Task<Attempt<TS2>> MapSuccess<TS, TS2>(
        this Task<Attempt<TS>> attemptTask, Func<TS, TS2> mapper, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.MapSuccess(mapper), cancellationToken);
    }
    public static Task<Attempt<TS2>> FlatMapSuccess<TS, TS2>(
        this Task<Attempt<TS>> attemptTask, Func<TS, Attempt<TS2>> mapper, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.FlatMapSuccess(mapper), cancellationToken);
    }
        
    public static Task<Attempt> Simplify<TS>(
        this Task<Attempt<TS>> attemptTask, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.Simplify(), cancellationToken);
    }
}