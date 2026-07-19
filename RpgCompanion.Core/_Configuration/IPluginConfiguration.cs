namespace RpgCompanion.Core;

public interface IPluginConfiguration
{
    public IPluginConfiguration WithName(string name);
    public IPluginConfiguration WithVersion(string version);

    public IPluginConfiguration WithInitialization(InitializationHandler handler);
    public IPluginConfiguration WithInitialization(InitializationAsyncHandler handler);

    public IPluginConfiguration AddIntent<TIntent>(IntentHandler<TIntent> handler) where TIntent : IIntent;
    public IPluginConfiguration AddIntent<TIntent>(IntentHandlerAsync<TIntent> handler) where TIntent : IIntent;
    public IPluginConfiguration AddIntent<TIntent, TResult>(IntentHandler<TIntent, TResult> handler) where TIntent : IIntent<TResult>;
    public IPluginConfiguration AddIntent<TIntent, TResult>(IntentHandlerAsync<TIntent, TResult> handler) where TIntent : IIntent<TResult>;
}
