namespace RpgCompanion.Host;

internal class InitializedPluginMetadata : LoadedPluginMetadata
{
    private InitializedPluginMetadata(LoadedPluginMetadata metadata) : base(metadata)
    {
    }

    public static InitializedPluginMetadata Create(LoadedPluginMetadata metadata)
    {
        return new InitializedPluginMetadata(metadata);
    }
}
