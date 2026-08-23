namespace RpgCompanion.Core;

public interface IAsyncRule<TSubject>
{
    Task<RuleResult<TSubject>> Apply(TSubject subject, IAsyncRuleContext context);
}

public interface IAsyncRule<TSubject, TResult>
{
    Task<RuleResult<TResult>> Apply(TSubject subject, IAsyncRuleContext context);
}
