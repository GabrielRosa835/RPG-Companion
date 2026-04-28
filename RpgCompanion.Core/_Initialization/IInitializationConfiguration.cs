namespace RpgCompanion.Core;

public interface IInitializationConfiguration
{
    public IInitializationConfiguration Export(Initialization instance);
    public IInitializationConfiguration Export(Func<IRegistry, InitializationKey, Initialization> factory);
}
