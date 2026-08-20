namespace RpgCompanion.Host;

internal class InitializationContextFactory(ScopeProvider scopeProvider, IRegistry registry)
{
    public InitializationContext Create(CancellationToken ct)
    {
        return new InitializationContext
        {
            Scope = scopeProvider.CreateScope(),
            Registry = registry,
            CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(ct)
        };
    }
}
