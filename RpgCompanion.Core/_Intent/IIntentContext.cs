namespace RpgCompanion.Core;

public interface IIntentContext
{
    /// <summary>
    /// Grants access to scoped dependencies for the current operation.
    /// </summary>
    IRegistry Registry { get; }

    IHostContext Host { get; }
}
