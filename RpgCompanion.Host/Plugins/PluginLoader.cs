namespace RpgCompanion.Host;

using System.Runtime.Loader;
using Configuration;
using MongoDB.Bson.Serialization;

public class PluginLoader
{
    private readonly IServiceCollection _services;
    private readonly string _sourcesFolder;

    public PluginLoader(IServiceCollection services, string sourcesFolder)
    {
        if (!Directory.Exists(sourcesFolder))
        {
            throw new DirectoryNotFoundException(sourcesFolder);
        }
        _services = services;
        _sourcesFolder = sourcesFolder;
    }

    internal async Task<List<PluginMetadata>> LoadAll()
    {
        var plugins = new List<PluginMetadata>();

        foreach (var file in Directory.GetFiles(_sourcesFolder, "*.dll", SearchOption.AllDirectories))
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            if (plugins.Any(p => p.Resource != fileName))
            {
                continue;
            }
            plugins.Add(new PluginMetadata(file));
        }

        var loadPluginsTasks = plugins.Select(LoadSingle).ToArray();

        await Task.WhenAll(loadPluginsTasks);

        return loadPluginsTasks.Select(t => t.Result).ToList();
    }

    private Task<PluginMetadata> LoadSingle(PluginMetadata metadata) => Task.Run(() =>
    {
        var context = new AssemblyLoadContext(metadata.Resource, isCollectible: true);
        var assembly = context.LoadFromAssemblyPath(metadata.FilePath);

        var (manifestType, _) = ProcessAssemblyTypes(assembly.GetTypes());

        if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
        {
            throw new InvalidOperationException($"Could not find manifest for plugin {metadata.Resource}");
        }

        var configuration = new PluginConfiguration(_services, metadata);
        manifest.Configure(configuration);
        metadata.Descriptor = configuration.Build();
        metadata.Assembly = assembly;
        metadata.Activated = true;
        return metadata;
    });

    private (Type? ManifestType, bool unused) ProcessAssemblyTypes(IEnumerable<Type> types)
    {
        Type? manifestType = null;

        foreach (var type in types)
        {
            if (type.Implements(typeof(IManifest)))
            {
                manifestType = type;
                continue;
            }
            if (type.Implements(typeof(IEntity)))
            {
                var cm = new BsonClassMap(type);
                cm.AutoMap();
                cm.MapIdProperty(nameof(IEntity.DbId));
                BsonClassMap.RegisterClassMap(cm);

                var subtypeAttributes = type
                    .GetCustomAttributes(typeof(HasSubtypeAttribute), false)
                    .OfType<HasSubtypeAttribute>()
                    .ToList();

                if (subtypeAttributes.Count > 0)
                {
                    foreach (var attr in subtypeAttributes)
                    {
                        cm.AddKnownType(attr.KnownType);
                    }
                }
            }
        }

        return (manifestType, true);
    }
}

file static class SelfUtils
{
    public static bool Implements(this Type type, Type interfaceType)
    {
        return !(type.IsInterface || type.IsAbstract) && type.GetInterfaces().Contains(interfaceType);
    }
}
