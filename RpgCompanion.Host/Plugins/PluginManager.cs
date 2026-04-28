namespace RpgCompanion.Host.Plugins;

using System.Collections.Concurrent;
using System.Runtime.Loader;
using Configuration;
using Core;
using Microsoft.Extensions.DependencyInjection;

public class PluginManager
{
    private ConcurrentBag<PluginKey> _plugins { get; } = [];
    public IReadOnlyList<PluginKey> Plugins => _plugins.ToArray();

    public Task LoadAll(IServiceCollection services, string pluginsFolder,
        CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(FindAll(pluginsFolder).Select(m => Load(services, m, cancellationToken)));
    }

    public Task InitializeAll(IServiceProvider provider, CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(_plugins.Select(p => Initialize(provider, p, cancellationToken)));
    }

    private IReadOnlyList<PluginMetadata> FindAll(string pluginsFolder)
    {
        if (!Directory.Exists(pluginsFolder))
        {
            throw new DirectoryNotFoundException(pluginsFolder);
        }

        var plugins = new List<PluginMetadata>();

        foreach (var file in Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.AllDirectories))
        {
            if (plugins.Any(p => p.Resource != Path.GetFileNameWithoutExtension(file)))
            {
                continue;
            }
            plugins.Add(new PluginMetadata(file));
        }

        return plugins;
    }

    private Task Load(IServiceCollection services, PluginMetadata metadata,
        CancellationToken cancellationToken = default) => Task.Run(() =>
        {
            var context = new AssemblyLoadContext(metadata.Resource, isCollectible: true);
            var assembly = context.LoadFromAssemblyPath(metadata.FilePath);

            var assemblyTypes = assembly.GetTypes();
            var manifestType = assemblyTypes.FirstOrDefault(t => t.Implements(typeof(IManifest)));

            if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
            {
                throw new InvalidOperationException($"Could not find manifest for plugin {metadata.Resource}");
            }

            var pluginBuilder = new PluginConfiguration(services, metadata);
            manifest.Configure(pluginBuilder);
            var descriptor = pluginBuilder.Build();

            _plugins.Add(descriptor.Key);
        },
        cancellationToken);

    private Task Initialize(IServiceProvider provider, PluginKey key, CancellationToken cancellationToken = default) =>
        Task.Run(() =>
            {
                var initializer = provider.GetRequiredKeyedService<IInitialization>(key);
                var registry = provider.GetRequiredKeyedService<IRegistry>(key);
                initializer.Initialize(registry);
            },
            cancellationToken);
}

file static class SelfUtils
{
    public static bool Implements(this Type type, Type interfaceType)
    {
        return !(type.IsInterface || type.IsAbstract) && type.GetInterfaces().Contains(interfaceType);
    }
}
