namespace RpgCompanion.Core;

internal readonly struct Rule<TSubject>(Func<TSubject, IRuleContext, RuleResult<TSubject>> Handler) : IRule<TSubject>
{
    public RuleResult<TSubject> Apply(TSubject subject, IRuleContext context) => Handler(subject, context);
}

internal readonly struct Rule<TSubject, TResult>(Func<TSubject, IRuleContext, RuleResult<TResult>> Handler) : IRule<TSubject, TResult>
{
    public RuleResult<TResult> Apply(TSubject subject, IRuleContext context) => Handler(subject, context);
}

internal readonly struct AsyncRule<TSubject>(Func<TSubject, IAsyncRuleContext, Task<RuleResult<TSubject>>> Handler) : IAsyncRule<TSubject>
{
    public Task<RuleResult<TSubject>> Apply(TSubject subject, IAsyncRuleContext context) => Handler(subject, context);
}

internal readonly struct AsyncRule<TSubject, TResult>(Func<TSubject, IAsyncRuleContext, Task<RuleResult<TResult>>> Handler) : IAsyncRule<TSubject, TResult>
{
    public Task<RuleResult<TResult>> Apply(TSubject subject, IAsyncRuleContext context) => Handler(subject, context);
}
