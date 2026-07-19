namespace RpgCompanion.Core;

public abstract class IntentContext
{
    /// <summary>
    /// Grants access to scoped dependencies for the current operation.
    /// </summary>
    public abstract IRegistry Registry { get; }
}
