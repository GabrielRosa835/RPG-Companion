namespace RpgCompanion.Host;

public class RuleApplier(ScopeManager _scopeManager) : IRuleApplier
{
    public RuleResult<TSubject> Apply<TSubject>(TSubject subject, IRule<TSubject> rule)
    {
        using var scope = _scopeManager.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<RuleContextFactory>();
        var ctx = factory.Create();
        return rule.Apply(subject, ctx);
    }

    public RuleResult<TResult> Apply<TSubject, TResult>(TSubject subject, IRule<TSubject, TResult> rule)
    {
        using var scope = _scopeManager.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<RuleContextFactory>();
        var ctx = factory.Create();
        return rule.Apply(subject, ctx);
    }

    public async Task<RuleResult<TSubject>> Apply<TSubject>(TSubject subject, IAsyncRule<TSubject> rule, CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeManager.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<RuleContextFactory>();
        var ctx = factory.Create(cancellationToken);
        return await rule.Apply(subject, ctx);
    }

    public async Task<RuleResult<TResult>> Apply<TSubject, TResult>(TSubject subject, IAsyncRule<TSubject, TResult> rule, CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeManager.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<RuleContextFactory>();
        var ctx = factory.Create(cancellationToken);
        return await rule.Apply(subject, ctx);
    }
}
