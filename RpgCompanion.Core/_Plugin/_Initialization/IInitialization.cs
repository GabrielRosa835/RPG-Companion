namespace RpgCompanion.Core;

public interface IInitialization
{
    void Initialize(IInitializationContext context);
}

public interface IAsyncInitialization
{
    Task Initialize(IInitializationContext context, CancellationToken cancellationToken);
}
