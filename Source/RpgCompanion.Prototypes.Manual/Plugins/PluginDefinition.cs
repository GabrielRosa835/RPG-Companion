namespace RpgCompanion.Application;

using Services;

internal class PluginDefinition
{
    public IServiceProvider Services { get; set; } = default!;
    public ComponentCollection Components { get; set; } = default!;
    public PluginMetadata Metadata { get; set; } = default!;
}
