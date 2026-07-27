namespace RpgCompanion.Host.HostExclusive;

internal class HostRegistry(PluginArchives _plugins) : IHostRegistry
{
    public TService? Find<TService>(PluginKey pluginKey) where TService : class
    {
        return _plugins[pluginKey].Services.GetService<TService>();
    }

    public TService Get<TService>(PluginKey pluginKey) where TService : class
    {
        return _plugins[pluginKey].Services.GetRequiredService<TService>();
    }
}
