namespace RpgCompanion.Toolbox;

public static class AttemptAsync
{
    extension(Task<Attempt> attemptTask)
    {
        public async Task<TR> Either<TR>(
            Func<TR> onSuccess,
            Func<Exception, TR> onFailure,
            CancellationToken cancellationToken = default)
        {
            var result = await attemptTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.Either(onSuccess, onFailure);
        }

        public Task<bool> IsFailure => attemptTask.GetIsFailure();
        public Task<bool> IsSuccess => attemptTask.GetIsSuccess();

        private async Task<bool> GetIsFailure()
        {
            var result = await attemptTask.ConfigureAwait(false);
            return result.IsFailure;
        }

        private async Task<bool> GetIsSuccess()
        {
            var result = await attemptTask.ConfigureAwait(false);
            return result.IsSuccess;
        }
    }

    extension<TS>(Task<Attempt<TS>> attemptTask)
    {
        public async Task<TR> Either<TR>(
            Func<TS, TR> onSuccess,
            Func<Exception, TR> onFailure,
            CancellationToken cancellationToken = default)
        {
            var result = await attemptTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.Either(onSuccess, onFailure);
        }

        public async Task<Attempt<TS2>> MapSuccess<TS2>(
            Func<TS, TS2> mapper,
            CancellationToken cancellationToken = default)
        {
            var result = await attemptTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.MapSuccess(mapper);
        }

        public async Task<Attempt<TS2>> FlatMapSuccess<TS2>(
            Func<TS, Attempt<TS2>> mapper,
            CancellationToken cancellationToken = default)
        {
            var result = await attemptTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.FlatMapSuccess(mapper);
        }

        public async Task<Attempt> Simplify(
            CancellationToken cancellationToken = default)
        {
            var result = await attemptTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.Simplify();
        }
    }

    extension<TS, TF>(Task<Attempt<TS, TF>> attemptTask)
    {
        public async Task<TR> Either<TR>(
            Func<TS, TR> onSuccess,
            Func<TF, TR> onFailure,
            CancellationToken cancellationToken = default)
        {
            var result = await attemptTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.Either(onSuccess, onFailure);
        }

        public async Task<Attempt<TS>> Simplify(
            CancellationToken cancellationToken = default)
        {
            var result = await attemptTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.Simplify();
        }
    }
}
