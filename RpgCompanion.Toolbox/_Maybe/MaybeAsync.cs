namespace RpgCompanion.Toolbox;

public static class MaybeAsync
{
    extension<T>(Task<Maybe<T>> maybeTask)
    {
        public async Task<TR> Either<TR>(
            Func<T, TR> onSome,
            Func<TR> onEmpty,
            CancellationToken cancellationToken = default)
        {
            var result = await maybeTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.Either(onSome, onEmpty);
        }
    }
}
