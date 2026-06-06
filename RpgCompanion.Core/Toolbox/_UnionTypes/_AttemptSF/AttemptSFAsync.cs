namespace RpgCompanion.Core.Toolbox;

public static class AttemptSFAsync
{
    extension<TS, TF>(Task<Attempt<TS, TF>> attemptTask)
    {
        public Task<TR> Either<TR>(Func<TS, TR> onSuccess, Func<TF, TR> onFailure, CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.Either(onSuccess, onFailure), cancellationToken);
        }

        public Task<Attempt<TS>> Simplify(CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.Simplify(), cancellationToken);
        }
    }
}
