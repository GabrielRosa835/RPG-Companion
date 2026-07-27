namespace RpgCompanion.Core;

public interface IInitializationBase;

public interface IInitialization : IInitializationBase
{
    void Initialize(IInitializationContext context);
}

public interface IAsyncInitialization : IInitializationBase
{
    Task Initialize(IInitializationContext context, CancellationToken cancellationToken);
}
