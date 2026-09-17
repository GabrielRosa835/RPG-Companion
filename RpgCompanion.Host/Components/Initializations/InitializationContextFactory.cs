namespace RpgCompanion.Host;

internal class InitializationContextFactory(ScopeManager scopeManager, IRegistry registry)
{
    public InitializationContext Create(CancellationToken ct)
    {
        return new InitializationContext
        {
            Scope = scopeManager.CreateScope(),
            Registry = registry,
            CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(ct)
        };
    }
}
