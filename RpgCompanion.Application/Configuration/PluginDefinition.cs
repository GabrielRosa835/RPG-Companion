namespace RpgCompanion.Application;

using Services;

internal class PluginDefinition
{
    public IServiceProvider Services { get; set; }
    public ComponentCollection Components { get; set; }
    public PluginMetadata Metadata { get; set; }
}
