namespace RpgCompanion.Application;

using Core.Meta;
using Microsoft.Extensions.DependencyInjection;

internal class PluginMetadataBuilder(IServiceCollection services) : IPluginMetadataBuilder
{
    private PluginMetadata _metadata = new();
    public PluginMetadata Build() => _metadata;

    public IPluginMetadataBuilder WithInitializer<TInitializer>() where TInitializer : class, IInitializer
    {
        services.AddTransient<TInitializer>();
        _metadata.InitializerType = typeof(TInitializer);
        return this;
    }

    public IPluginMetadataBuilder WithName(string name)
    {
        _metadata.PluginName = name;
        return this;
    }

    public IPluginMetadataBuilder WithVersion(string version)
    {
        _metadata.PluginVersion = version;
        return this;
    }

    public IPluginMetadataBuilder WithInitialization(InitializerAction action)
    {
        _metadata.Initialization = action;
        return this;
    }
}
