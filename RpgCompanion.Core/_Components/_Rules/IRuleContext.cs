namespace RpgCompanion.Core;

public interface IRuleContext
{
    public IRegistry Registry { get; }
}

public interface IAsyncRuleContext : IRuleContext
{
    public CancellationToken CancellationToken { get; }
}
