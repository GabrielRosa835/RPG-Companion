namespace RpgCompanion.Host;

internal class RuleContext : IAsyncRuleContext
{
    internal required CancellationTokenSource CancellationSource { get; init; }
    public required IRegistry Registry { get; init; }
    public CancellationToken CancellationToken => CancellationSource.Token;
}
