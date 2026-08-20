namespace RpgCompanion.Host;

using System.Reflection;

internal class LoadedPluginMetadata : PluginMetadata
{
    internal PluginLoadContext LoadContext { get; private init; } = default!;
    internal Assembly Assembly { get; private init; } = default!;
    internal PluginDescriptor Descriptor { get; private init; } = default!;
    internal IServiceProvider Services { get; private init; } = default!;

    private LoadedPluginMetadata(PluginMetadata metadata) : base(metadata)
    {
    }

    protected LoadedPluginMetadata(LoadedPluginMetadata metadata) : base(metadata)
    {
        LoadContext = metadata.LoadContext;
        Assembly = metadata.Assembly;
        Descriptor = metadata.Descriptor;
        LoadContext = metadata.LoadContext;
    }

    public static LoadedPluginMetadata Create(
        PluginMetadata metadata,
        PluginLoadContext loadContext,
        Assembly assembly,
        PluginDescriptor descriptor,
        IServiceProvider services)
    {
        return new LoadedPluginMetadata(metadata)
        {
            LoadContext = loadContext,
            Assembly = assembly,
            Descriptor = descriptor,
            Services = services,
        };
    }
}
