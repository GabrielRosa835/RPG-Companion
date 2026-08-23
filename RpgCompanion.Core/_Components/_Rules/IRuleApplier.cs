namespace RpgCompanion.Core;

public interface IRuleApplier
{
    public RuleResult<TSubject> Apply<TSubject>(TSubject subject, IRule<TSubject> rule);
    public RuleResult<TResult> Apply<TSubject, TResult>(TSubject subject, IRule<TSubject, TResult> rule);
    public Task<RuleResult<TSubject>> Apply<TSubject>(TSubject subject, IAsyncRule<TSubject> rule);
    public Task<RuleResult<TResult>> Apply<TSubject, TResult>(TSubject subject, IAsyncRule<TSubject, TResult> rule);
}
