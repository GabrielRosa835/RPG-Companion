namespace RpgCompanion.Host.Plugins;

using System.Runtime.Loader;
using Configuration;
using Core.Meta;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class PluginLoader(PluginManager _manager, IServiceCollection _services)
{
    internal Task<PluginDescriptor> Load(PluginMetadata metadata) => Task.Run(() =>
    {
        var context = new AssemblyLoadContext(metadata.Resource, isCollectible: true);
        var assembly = context.LoadFromAssemblyPath(metadata.FilePath);

        var assemblyTypes = assembly.GetTypes();
        var manifestType = assemblyTypes.FirstOrDefault(t => t.Implements(typeof(IManifest)));

        if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
        {
            throw new InvalidOperationException($"Could not find manifest for plugin {metadata.Resource}");
        }

        var pluginBuilder = new PluginConfiguration(_services, new PluginKey(), metadata);
        manifest.Configure(pluginBuilder as IPluginBuilder);
        var descriptor = pluginBuilder.Build();

        _manager.Plugins.Add(descriptor.Key);

        return descriptor;
    });
}

file static class SelfUtils
{
    public static bool Implements(this Type type, Type interfaceType)
    {
        return !(type.IsInterface || type.IsAbstract) && type.GetInterfaces().Contains(interfaceType);
    }
}
