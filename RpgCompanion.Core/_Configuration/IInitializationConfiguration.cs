namespace RpgCompanion.Core;

public interface IInitializationConfiguration
{
    public IInitializationConfiguration WithComponent<TInitializer>() where TInitializer : class, IInitializer;
    public IInitializationConfiguration WithAction(InitializerAction action);
    public IInitializationConfiguration WithComponentAsync<TInitializer>() where TInitializer : class, IAsyncInitializer;
    public IInitializationConfiguration WithActionAsync(AsyncInitializerAction action);
}
