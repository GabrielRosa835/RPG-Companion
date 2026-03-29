namespace RpgCompanion.Application;

using System.Runtime.Loader;
using Core.Engine;
using Core.Meta;
using Utils.UnionTypes;
using Microsoft.Extensions.DependencyInjection;

// Singleton
internal class PluginLoader(
    PluginCollection plugins, // Singleton
    IServiceProvider hostServices) // Singleton
{
    internal Task<Attempt> Load(PluginDescriptor plugin)
    {
        if (plugin.Activated)
        {
            return Task.FromResult(Results.Success());
        }

        return LoadInternal(plugin);
    }

    internal Task<Attempt> ForceLoad(PluginDescriptor plugin)
    {
        return LoadInternal(plugin);
    }

    private Task<Attempt> LoadInternal(PluginDescriptor plugin) => Task.Run(() =>
    {
        try
        {
            var context = new AssemblyLoadContext(plugin.Resource, isCollectible: true);
            var assembly = context.LoadFromAssemblyPath(plugin.FilePath);

            var assemblyTypes = assembly.GetTypes();
            var manifestType = assemblyTypes.FirstOrDefault(t => t.Implements(typeof(IManifest)));

            if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
            {
                return Results.Failure();
            }

            var pluginBuilder = hostServices.GetRequiredService<PluginBuilder>();
            manifest.Configure(pluginBuilder);
            var definition = pluginBuilder.Build();

            plugin.Services = definition.Services;
            plugin.Assembly = assembly;
            plugin.Activated = true;
            plugin.Identifier = new()
            {
                Name = definition.Metadata.PluginName ?? plugin.Resource,
                Version = definition.Metadata.PluginVersion ?? "[VERSION NOT DEFINED]",
            };

            var registry = plugin.Services.GetRequiredService<IRegistry>();
            plugin.Services.GetService<InitializerAction>()?.Invoke(registry);
            plugin.Services.GetService<IInitializer>()?.Initialize(registry);

            return Results.Success();
        }
        catch (Exception e)
        {
            return Results.Failure(e);
        }
    });
}

file static class SelfUtils
{
    public static bool Implements(this Type type, Type interfaceType)
    {
        return !(type.IsInterface || type.IsAbstract) && type.GetInterfaces().Contains(interfaceType);
    }
}
