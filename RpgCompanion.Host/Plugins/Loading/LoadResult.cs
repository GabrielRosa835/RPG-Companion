namespace RpgCompanion.Host;

internal interface LoadResult
{
    internal readonly record struct None : LoadResult;
    internal readonly record struct Completed(LoadedPluginMetadata Metadata) : LoadResult;
    internal readonly record struct Faulted(Exception Exception) : LoadResult;
}
