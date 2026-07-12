namespace RpgCompanion.Toolbox;

public static class AttemptAsync
{
    extension(Task<Attempt> attemptTask)
    {
        public Task<TR> Either<TR>(Func<TR> onSuccess, Func<Exception, TR> onFailure, CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.Either(onSuccess, onFailure), cancellationToken);
        }

        public Task<bool> IsFailure()
        {
            return attemptTask.ContinueWith(task => task.Result.IsFailure);
        }

        public Task<bool> IsSuccess ()
        {
            return attemptTask.ContinueWith(task => task.Result.IsSuccess);
        }
    }
}
