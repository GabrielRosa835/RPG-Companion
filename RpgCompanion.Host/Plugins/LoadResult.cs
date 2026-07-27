namespace RpgCompanion.Host;

internal static class LoadResult
{
    internal static ILoadResult None => new ILoadResult.None();
    internal static ILoadResult Completed(PluginMetadata metadata) => new ILoadResult.Completed(metadata);
    internal static ILoadResult Faulted(Exception e) => new ILoadResult.Faulted(e);
}

internal interface ILoadResult
{
    internal readonly record struct None : ILoadResult;
    internal readonly record struct Completed(PluginMetadata Metadata) : ILoadResult;
    internal readonly record struct Faulted(Exception e) : ILoadResult;
}
