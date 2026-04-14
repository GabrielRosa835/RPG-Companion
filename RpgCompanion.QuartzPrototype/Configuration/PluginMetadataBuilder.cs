namespace RpgCompanion.QuartzPrototype.Configuration;

using Plugins;
using RpgCompanion.Core.Meta;

internal class PluginMetadataBuilder(PluginMetadata metadata) : IPluginMetadataBuilder
{
    public IPluginMetadataBuilder WithName(string name)
    {
        metadata.PluginName = name;
        return this;
    }

    public IPluginMetadataBuilder WithVersion(string version)
    {
        metadata.PluginVersion = version;
        return this;
    }
}
