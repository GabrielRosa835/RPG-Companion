namespace RpgCompanion.Core;

using Core;

public delegate void InitializationHandler (InitializationContext context);
public delegate Task InitializationAsyncHandler (InitializationContext context, CancellationToken cancellationToken = default);

public abstract class InitializationContext
{
    public abstract IRegistry Registry { get; }
}
