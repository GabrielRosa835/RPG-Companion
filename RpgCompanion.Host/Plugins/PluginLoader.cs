namespace RpgCompanion.Host;

using System.Reflection;
using Configuration;
using HostExclusive;

internal interface LoadResult
{
    internal readonly record struct None : LoadResult;
    internal readonly record struct Completed(PluginMetadata Metadata) : LoadResult;
    internal readonly record struct Faulted(Exception Exception) : LoadResult;
}

internal class PluginLoader(
    IServiceProvider _hostServices,
    PluginArchives _pluginArchives,
    EventArchives _eventArchives,
    IntentArchives _intentArchives,
    EntityArchives _entityArchives)
{
    internal async Task<List<LoadResult>> LoadMany(IEnumerable<PluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        var loadTasks = plugins.Select(p => LoadSingle(p, cancellationToken)).ToList();
        await Task.WhenAll(loadTasks);
        return loadTasks.Select(t => t.Result).ToList();
    }

    internal Task<LoadResult> LoadSingle(PluginMetadata metadata, CancellationToken cancellationToken = default) => Task.Run<LoadResult>(() =>
    {
        try
        {
            // Verify the bin/dll file actually exists before trying to load it
            if (!File.Exists(metadata.EntryPointPath))
            {
                return new LoadResult.Faulted(new FileNotFoundException($"Plugin entry point not found at {metadata.EntryPointPath}"));
            }

            // 1. Use your custom context and pass the plugin ID and specific DLL path
            var context = new PluginLoadContext(metadata.Manifest.Id, metadata.EntryPointPath);

            // 2. Load the assembly into the custom context
            var assembly = context.LoadFromAssemblyPath(metadata.EntryPointPath);

            var manifestType = ProcessAssemblyTypes(assembly);

            if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
            {
                return new LoadResult.Faulted(new InvalidOperationException($"Could not find IManifest implementation for plugin {metadata.Manifest.Id}"));
            }

            IServiceCollection services = new ServiceCollection();
            services.AddPluginServices(_hostServices);

            var configuration = new PluginConfiguration(services,
                _eventArchives,
                _intentArchives,
                _entityArchives);

            var hostRegistry = new HostRegistry(_hostServices, _pluginArchives);
            var hostContext = new HostContext(hostRegistry);

            manifest.Configure(configuration, hostContext);

            metadata.Descriptor = configuration.Build();
            metadata.Services = services.BuildServiceProvider();
            metadata.Assembly = assembly;
            metadata.LoadContext = context;
            metadata.Loaded = true;

            _pluginArchives.Add(metadata);

            return new LoadResult.Completed(metadata);
        }
        catch (Exception e)
        {
            return new LoadResult.Faulted(e);
        }
    },
    cancellationToken);

    internal Type? ProcessAssemblyTypes(Assembly assembly)
    {
        Type? manifestType = null;

        foreach (var type in assembly.GetTypes())
        {
            if (type.Implements(typeof(IManifest)))
            {
                manifestType = type;
            }
            // Process other assembly types
        }

        return manifestType;
    }
}

file static class Extensions
{
    internal static bool Implements(this Type type, Type interfaceType)
    {
        return !(type.IsInterface || type.IsAbstract) && type.GetInterfaces().Contains(interfaceType);
    }
}
