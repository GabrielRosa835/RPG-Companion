namespace RpgCompanion.Core;

public readonly struct Rule<TSubject>(Func<TSubject, IRuleContext, TSubject> Handler) : IRule<TSubject>
{
    public TSubject Apply(TSubject subject, IRuleContext context) => Handler(subject, context);
}

public readonly struct Rule<TSubject, TResult>(Func<TSubject, IRuleContext, TResult> Handler) : IRule<TSubject, TResult>
{
    public TResult Apply(TSubject subject, IRuleContext context) => Handler(subject, context);
}

public readonly struct AsyncRule<TSubject>(Func<TSubject, IRuleContext, Task<TSubject>> Handler) : IAsyncRule<TSubject>
{
    public Task<TSubject> Apply(TSubject subject, IRuleContext context) => Handler(subject, context);
}

public readonly struct AsyncRule<TSubject, TResult>(Func<TSubject, IRuleContext, Task<TResult>> Handler) : IAsyncRule<TSubject, TResult>
{
    public Task<TResult> Apply(TSubject subject, IRuleContext context) => Handler(subject, context);
}
