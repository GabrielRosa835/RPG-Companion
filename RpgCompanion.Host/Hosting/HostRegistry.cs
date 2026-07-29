namespace RpgCompanion.Host.HostExclusive;

internal class HostRegistry(
    IServiceProvider _serviceProvider,
    PluginArchives _plugins) : IHostRegistry
{
    public TService? Find<TService>() where TService : class
    {
        return _serviceProvider.GetService<TService>();
    }

    public TService? Find<TService>(PluginKey pluginKey) where TService : class
    {
        return _plugins[pluginKey].Services.GetService<TService>();
    }

    public TService Get<TService>() where TService : class
    {
        return _serviceProvider.GetRequiredService<TService>();
    }

    public TService Get<TService>(PluginKey pluginKey) where TService : class
    {
        return _plugins[pluginKey].Services.GetRequiredService<TService>();
    }
}
