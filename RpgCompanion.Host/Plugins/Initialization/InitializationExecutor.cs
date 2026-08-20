namespace RpgCompanion.Host;

internal class InitializationExecutor(IServiceProvider _services)
{
    internal async Task<InitializationType> Initialize(IInitializationContext context, CancellationToken cancellationToken)
    {
       IAsyncInitialization? asyncInitialization = _services.GetService<IAsyncInitialization>();

       if (asyncInitialization is not null)
       {
            await asyncInitialization.Initialize(context, cancellationToken);
            return InitializationType.Async;
       }

       IInitialization? syncInitialization = _services.GetService<IInitialization>();

       if (syncInitialization is not null)
       {
           syncInitialization!.Initialize(context);
           return InitializationType.Sync;
       }

       return InitializationType.None;
    }
}
