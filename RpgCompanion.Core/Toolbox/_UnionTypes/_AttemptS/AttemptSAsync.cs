namespace RpgCompanion.Core.Toolbox;

public static class AttemptSAsync
{
    extension<TS>(Task<Attempt<TS>> attemptTask)
    {
        public Task<TR> Either<TR>(Func<TS, TR> onSuccess, Func<Exception, TR> onFailure, CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.Either(onSuccess, onFailure), cancellationToken);
        }

        public Task<Attempt<TS2>> MapSuccess<TS2>(Func<TS, TS2> mapper, CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.MapSuccess(mapper), cancellationToken);
        }

        public Task<Attempt<TS2>> FlatMapSuccess<TS2>(Func<TS, Attempt<TS2>> mapper, CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.FlatMapSuccess(mapper), cancellationToken);
        }

        public Task<Attempt> Simplify(CancellationToken cancellationToken = default)
        {
            return attemptTask.ContinueWith(task => task.Result.Simplify(), cancellationToken);
        }
    }
}
