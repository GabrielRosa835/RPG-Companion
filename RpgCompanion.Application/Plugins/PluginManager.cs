namespace RpgCompanion.Application;

using System.Runtime.Loader;
using Core.Events;
using Core.Meta;
using Microsoft.Extensions.DependencyInjection;
using Reflection;
using Utils.UnionTypes;

internal class PluginManager(IServiceProvider appServices)
{
    private readonly List<PluginDescriptor> _descriptors = [];
    public IReadOnlyList<PluginDescriptor> Descriptors => _descriptors.AsReadOnly();

    public PluginDescriptor this[int index] => _descriptors[index];
    public PluginDescriptor this[string name] => _descriptors.First(d => d.Resource == name);
    public PluginDescriptor this[Type eventType] => _descriptors.First(d => d.Events.Contains(eventType));

    public IReadOnlyList<PluginDescriptor> FindPlugins(string pluginsFolder)
    {
        if (!Directory.Exists(pluginsFolder))
        {
            throw new DirectoryNotFoundException(pluginsFolder);
        }

        _descriptors.AddRange(Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.AllDirectories)
            .Select(dll => new PluginDescriptor
            {
                Resource = Path.GetFileNameWithoutExtension(dll), Path = Path.GetFullPath(dll),
            })
            .Where(d => !_descriptors.Contains(d)));
        return Descriptors;
    }

    public Task<Attempt> Load(PluginDescriptor plugin)
    {
        if (plugin.Activated)
        {
            return Task.FromResult(Results.Success());
        }

        return LoadInternal(plugin, appServices);
    }

    public Task<Attempt> ForceLoad(PluginDescriptor plugin)
    {
        return LoadInternal(plugin, appServices);
    }

    internal static Task<Attempt> LoadInternal(PluginDescriptor plugin, IServiceProvider appServices) => Task.Run(() =>
    {
        try
        {
            var context = new AssemblyLoadContext(plugin.Resource, isCollectible: true);
            var assembly = context.LoadFromAssemblyPath(plugin.Path);

            var manifestType = assembly.GetTypes().FirstOrDefault(t => t.Implements(typeof(IManifest)));

            if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
            {
                return Results.Failure();
            }

            var services = new ServiceCollection();
            services.AddAppServices(appServices);
            var pluginBuilder = new PluginBuilder(services);
            manifest.Configure(pluginBuilder);
            var definition = pluginBuilder.Build();
            var registry = new ComponentProvider(definition.Components, definition.Services);

            plugin.Assembly = assembly;
            plugin.Identifier = new(definition.Metadata.PluginName, definition.Metadata.PluginVersion);
            plugin.Registry = registry;
            plugin.Activated = true;
            plugin.Events.AddRange(assembly.GetTypes()
                .Where(t => t.Implements(typeof(IEvent))));

            if (definition.Metadata.InitializerType is not null)
            {
                var initializer = (IInitializer?) definition.Services.GetService(definition.Metadata.InitializerType);
                initializer?.Initialize(registry);
            }

            definition.Metadata.Initialization?.Invoke(registry);

            return Results.Success();
        }
        catch (Exception e)
        {
            return Results.Failure(e);
        }
    });
}
