namespace RpgCompanion.Core;

public abstract class IntentContext
{
    /// <summary>
    /// Grants access to scoped dependencies for the current operation.
    /// </summary>
    public IRegistry Registry { get; protected set; }

    public CancellationToken CancellationToken { get; protected set; }
}
