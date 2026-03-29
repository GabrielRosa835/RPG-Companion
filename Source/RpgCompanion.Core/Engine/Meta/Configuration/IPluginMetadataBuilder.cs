namespace RpgCompanion.Core.Meta;

public interface IPluginMetadataBuilder
{
    public IPluginMetadataBuilder WithName(string name);
    public IPluginMetadataBuilder WithVersion(string version);
}
