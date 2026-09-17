namespace RpgCompanion.Host;

internal class RuleContextFactory(IRegistry _registry)
{
    internal RuleContext Create(CancellationToken cancellationToken = default)
    {
        return new RuleContext
        {
            Registry = _registry,
            CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken),
        };
    }
}
