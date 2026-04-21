namespace RpgCompanion.Host.Plugins;

using Core.Engine;
using Core.Meta;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class PluginActivator(PluginManager _manager, IServiceProvider _provider)
{
    public Task LoadAll()
    {
        foreach (var pluginKey in _manager.Plugins)
        {
            var initializer = _provider.GetRequiredKeyedService<IInitializer>(pluginKey);
            var registry = _provider.GetRequiredKeyedService<IRegistry>(pluginKey);
            initializer.Initialize(registry);
        }
        return Task.CompletedTask;
    }
}
