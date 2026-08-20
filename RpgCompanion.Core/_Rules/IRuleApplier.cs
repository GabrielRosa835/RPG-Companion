namespace RpgCompanion.Core;

public interface IRuleApplier
{
    public TSubject Apply<TSubject>(TSubject subject, IRule<TSubject> rule);
    public TResult Apply<TSubject, TResult>(TSubject subject, IRule<TSubject, TResult> rule);
    public Task<TSubject> Apply<TSubject>(TSubject subject, IAsyncRule<TSubject> rule);
    public Task<TResult> Apply<TSubject, TResult>(TSubject subject, IAsyncRule<TSubject, TResult> rule);
}
