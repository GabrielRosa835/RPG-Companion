namespace RpgCompanion.Host;

using System.Reflection;
using System.Runtime.Loader;

public class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public PluginLoadContext(
        string pluginResource,
        string pluginPath)
        : base(pluginResource, isCollectible: false) // true allows unloading
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // 1. Ask the resolver if it knows where this dependency is
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        if (assemblyPath != null)
        {
            // 2. Load the dependency from the plugin's folder into this ALC
            return LoadFromAssemblyPath(assemblyPath);
        }

        // 3. Return null to let the runtime fallback to the Default context (Host)
        return null;
    }
}
