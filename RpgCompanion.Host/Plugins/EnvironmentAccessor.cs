namespace RpgCompanion.Host;

internal class EnvironmentAccessor : IEnvironmentAccessor
{
    private static readonly AsyncLocal<IPluginContext?> _currentPlugin = new();
    private static readonly AsyncLocal<ICampaignContext?> _currentCampaign = new();
    private static readonly AsyncLocal<ISessionContext?> _currentSession = new();
    private static readonly AsyncLocal<ISceneContext?> _currentScene = new();

    public IPluginContext? CurrentPlugin
    {
        get => _currentPlugin.Value;
        internal set => _currentPlugin.Value = value;
    }

    public ICampaignContext? CurrentCampaign
    {
        get => _currentCampaign.Value;
        internal set => _currentCampaign.Value = value;
    }

    public ISessionContext? CurrentSession
    {
        get => _currentSession.Value;
        internal set => _currentSession.Value = value;
    }

    public ISceneContext? CurrentScene
    {
        get => _currentScene.Value;
        internal set => _currentScene.Value = value;
    }
}
