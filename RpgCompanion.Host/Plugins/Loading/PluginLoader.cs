namespace RpgCompanion.Host;

using System.Reflection;

internal class PluginLoader(
    IServiceProvider _hostServices,
    EventArchives _eventArchives,
    IntentArchives _intentArchives,
    EntityArchives _entityArchives) : IPluginLoader
{
    public async Task<List<LoadResult>> LoadMany(IEnumerable<PluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        var loadTasks = plugins.Select(p => LoadSingle(p, cancellationToken)).ToList();
        await Task.WhenAll(loadTasks);
        return loadTasks.Select(t => t.Result).ToList();
    }

    public Task<LoadResult> LoadSingle(PluginMetadata metadata, CancellationToken cancellationToken = default) => Task.Run(() =>
        {
            try
            {
                // 1. Use your custom context and pass the plugin ID and specific DLL path
                var loadContext = new PluginLoadContext(metadata.Manifest.Id, metadata.EntryPointPath);

                // 2. Load the assembly into the custom context
                var assembly = loadContext.LoadFromAssemblyPath(metadata.EntryPointPath);

                var manifestType = ProcessAssemblyTypes(assembly);

                if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
                {
                    return NoManifestImplementation(metadata);
                }

                IServiceCollection services = new ServiceCollection();
                services.AddPluginServices(_hostServices, metadata);

                var configuration = new PluginConfiguration(services,
                    _eventArchives,
                    _intentArchives,
                    _entityArchives);

                var hostRegistry = new Registry(_hostServices);
                manifest.Configure(configuration, hostRegistry);

                var descriptor = configuration.Build();
                var pluginServices = services.BuildServiceProvider();

                var loadedMetadata = LoadedPluginMetadata.Create(
                    metadata,
                    loadContext,
                    assembly,
                    descriptor,
                    pluginServices);

                return new LoadResult.Completed(loadedMetadata);
            }
            catch (Exception e)
            {
                return new LoadResult.Faulted(e);
            }
        },
        cancellationToken);

    private Type? ProcessAssemblyTypes(Assembly assembly)
    {
        Type? manifestType = null;

        foreach (var type in assembly.GetTypes())
        {
            if (type.ImplementsConcrete(typeof(IManifest)))
            {
                manifestType = type;
            }
            // Process other assembly types
        }

        return manifestType;
    }

    private LoadResult NoManifestImplementation(PluginMetadata metadata) =>
        new LoadResult.Faulted(new InvalidOperationException(
            $"Could not find IManifest implementation for plugin {metadata.Manifest.Id}"));
}

file static class Helpers
{
    internal static bool ImplementsConcrete(this Type type, Type interfaceType)
    {
        if (type is null || interfaceType is null || !interfaceType.IsInterface) return false;
        return !(type.IsInterface || type.IsAbstract) && type.GetInterfaces().Contains(interfaceType);
    }
}
