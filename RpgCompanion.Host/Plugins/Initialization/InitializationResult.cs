namespace RpgCompanion.Host;

internal interface InitializationResult
{
    internal readonly record struct None : InitializationResult;

    internal readonly record struct Completed(InitializedPluginMetadata Metadata, InitializationType Type) : InitializationResult;

    internal readonly record struct Faulted(Exception Exception) : InitializationResult;
}
