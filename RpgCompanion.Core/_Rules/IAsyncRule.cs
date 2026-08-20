namespace RpgCompanion.Core;

public interface IAsyncRule<TSubject>
{
    public Task<TSubject> Apply(TSubject subject, IRuleContext context);
}

public interface IAsyncRule<TSubject, TResult>
{
    public Task<TResult> Apply(TSubject subject, IRuleContext context);
}
