namespace RpgCompanion.Core;

public interface IHostRegistry
{
    TService? Find<TService>(PluginKey pluginKey) where TService : class;
    TService Get<TService>(PluginKey pluginKey) where TService : class;
}
