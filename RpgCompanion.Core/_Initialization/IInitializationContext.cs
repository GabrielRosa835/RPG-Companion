namespace RpgCompanion.Core;

public interface IInitializationContext
{
    IRegistry Registry { get; }
    IHostContext Host { get; }
}
