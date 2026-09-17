namespace RpgCompanion.Host;

internal class IntentContextFactory(IRegistry registry)
{
    internal IntentContext Create(CancellationToken cancellationToken = default)
    {
        return new IntentContext
        {
            Registry = registry,
            CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken),
        };
    }
}
