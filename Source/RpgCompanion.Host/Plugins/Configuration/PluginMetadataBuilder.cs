using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

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
