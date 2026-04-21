namespace RpgCompanion.Application;

using Utils.UnionTypes;

// Singleton
internal class PluginManager(
    PluginCollection plugins, // Singleton
    PluginLoader loader) // Singleton
{
    internal PluginCollection Collection => plugins;
    internal PluginLoader Loader => loader;

    public PluginCollection FindPlugins(string pluginsFolder)
    {
        if (!Directory.Exists(pluginsFolder))
        {
            throw new DirectoryNotFoundException(pluginsFolder);
        }

        plugins.AddRange(Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.AllDirectories)
            .Where(file => plugins
                .All(p => p.Resource != Path.GetFileNameWithoutExtension(file)))
            .Select(file => new PluginDescriptor(file)));

        return plugins;
    }

    public async Task<Attempt<PluginDescriptor>> TryLoadByEvent(Type eventType)
    {
        var plugin = this.Collection[eventType];
        var loadAttempt = await this.Loader.Load(plugin);
        return loadAttempt.TryGetFailure(out var failure)
            ? Results.Failure<PluginDescriptor>(failure)
            : Results.Success(plugin);
    }
}
