namespace RpgCompanion.Core.Meta;

public interface IPluginMetadataBuilder
{
    public IPluginMetadataBuilder WithInitializer<TInitializer>() where TInitializer : class, IInitializer;
    public IPluginMetadataBuilder WithName(string name);
    public IPluginMetadataBuilder WithVersion(string version);
    public IPluginMetadataBuilder WithInitialization(InitializerAction action);
}
