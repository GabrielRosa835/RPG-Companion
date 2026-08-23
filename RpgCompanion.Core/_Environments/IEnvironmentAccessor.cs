namespace RpgCompanion.Core;

public interface IEnvironmentAccessor
{
    public IPluginContext? CurrentPlugin { get; }
    public ICampaignContext? CurrentCampaign { get; }
    public ISessionContext? CurrentSession { get; }
    public ISceneContext? CurrentScene { get; }
}
