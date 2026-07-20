namespace RpgCompanion.Core;

public interface IIntentContext
{
    /// <summary>
    /// Grants access to scoped dependencies for the current operation.
    /// </summary>
    IRegistry Registry { get; }
}

public interface IIntentContextAsync : IIntentContext
{
    /// <summary>
    /// The operation's CancellationToken
    /// </summary>
    CancellationToken CancellationToken { get; }
}
