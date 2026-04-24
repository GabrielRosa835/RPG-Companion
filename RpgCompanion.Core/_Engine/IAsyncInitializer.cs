namespace RpgCompanion.Core;

public interface IAsyncInitializer
{
    Task Initialize (IRegistry registry);
}

public delegate Task AsyncInitializerAction(IRegistry registry);
