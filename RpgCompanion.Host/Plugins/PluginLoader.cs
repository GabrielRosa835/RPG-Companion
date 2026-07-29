namespace RpgCompanion.Host;

using System.Reflection;
using System.Runtime.Loader;
using Configuration;
using HostExclusive;
using MongoDB.Bson.Serialization;

internal class PluginLoader(
    IServiceProvider _hostServices,
    PluginArchives _pluginArchives,
    EventArchives _eventArchives,
    IntentArchives _intentArchives,
    EntityArchives _entityArchives)
{
    internal async Task<List<ILoadResult>> LoadMany(IEnumerable<PluginMetadata> plugins, CancellationToken cancellationToken = default)
    {
        var loadTasks = plugins.Select(p => LoadSingle(p, cancellationToken)).ToList();
        await Task.WhenAll(loadTasks);
        return loadTasks.Select(t => t.Result).ToList();
    }

    internal Task<ILoadResult> LoadSingle(PluginMetadata metadata, CancellationToken cancellationToken = default) => Task.Run(() =>
    {
        try
        {
            var context = new AssemblyLoadContext(metadata.Resource, isCollectible: true);
            var assembly = context.LoadFromAssemblyPath(metadata.FilePath);

            var manifestType = ProcessAssembly(assembly);

            if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
            {
                return LoadResult.Faulted(new InvalidOperationException($"Could not find manifest for plugin {metadata.Resource}"));
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
            metadata.Loaded = true;

            _pluginArchives.Add(metadata);

            return LoadResult.Completed(metadata);
        }
        catch (Exception e)
        {
            return LoadResult.Faulted(e);
        }
    },
    cancellationToken);

    internal Type? ProcessAssembly(Assembly assembly)
    {
        Type? manifestType = null;

        foreach (var type in assembly.GetTypes())
        {
            if (type.Implements(typeof(IManifest)))
            {
                manifestType = type;
                continue;
            }
            // if (type.Implements(typeof(IEntity)))
            // {
            //     var cm = new BsonClassMap(type);
            //     cm.AutoMap();
            //     cm.MapIdProperty(nameof(IEntity.DbId));
            //     BsonClassMap.RegisterClassMap(cm);
            //
            //     var subtypeAttributes = type
            //         .GetCustomAttributes(typeof(HasSubtypeAttribute), false)
            //         .OfType<HasSubtypeAttribute>()
            //         .ToList();
            //
            //     if (subtypeAttributes.Count > 0)
            //     {
            //         foreach (var attr in subtypeAttributes)
            //         {
            //             cm.AddKnownType(attr.KnownType);
            //         }
            //     }
            // }
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
