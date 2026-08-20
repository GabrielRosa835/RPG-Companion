namespace RpgCompanion.Host;

internal class PluginAccessor(PluginArchives _archives)
{
    public (LoadedPluginMetadata Metadata, PluginContext Context)  Get(object target)
    {
        LoadedPluginMetadata? currentPlugin = _archives[target.GetType().Assembly];

        if (currentPlugin is null)
        {
            throw new Exception();
        }

        var context = new PluginContext
        {
            Identifier = currentPlugin.Manifest.Id,
        };

        return (currentPlugin, context);
    }
}
