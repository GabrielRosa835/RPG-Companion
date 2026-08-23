namespace RpgCompanion.Host;

public class RuleApplierImpl(
    IServiceScopeFactory _scopeFactory)
    : IRuleApplier
{
    public RuleResult<TSubject> Apply<TSubject>(TSubject subject, IRule<TSubject> rule)
    {
        using var scope = _scopeFactory.CreateScope();
        var ctx = new RuleContextImpl();
        return rule.Apply(subject, ctx);
    }

    public TResult Apply<TSubject, TResult>(TSubject subject, IRule<TSubject, TResult> rule)
    {
        using var scope = _scopeFactory.CreateScope();
        var ctx = new RuleContextImpl();
        return rule.Apply(subject, ctx);
    }

    public async Task<TSubject> Apply<TSubject>(TSubject subject, IAsyncRule<TSubject> rule)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var ctx = new RuleContextImpl();
        return await rule.Apply(subject, ctx);
    }

    public async Task<TResult> Apply<TSubject, TResult>(TSubject subject, IAsyncRule<TSubject, TResult> rule)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var ctx = new RuleContextImpl();
        return await rule.Apply(subject, ctx);
    }
}
