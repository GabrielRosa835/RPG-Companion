namespace RpgCompanion.Host;

internal static class InitializationResult
{
    internal static IInitializationResult None => new IInitializationResult.None();
    internal static IInitializationResult Completed(PluginMetadata metadata, bool wasAsync) => new IInitializationResult.Completed(metadata, wasAsync);
    internal static IInitializationResult Faulted(Exception e) => new IInitializationResult.Faulted(e);
    internal static IInitializationResult NoInitializationFound => new IInitializationResult.NoInitializationFound();
}

internal interface IInitializationResult
{
    internal readonly record struct None : IInitializationResult;

    internal readonly record struct Completed(PluginMetadata Metadata, bool WasAsync) : IInitializationResult;

    internal readonly record struct Faulted(Exception Exception) : IInitializationResult;

    internal readonly record struct NoInitializationFound : IInitializationResult;
}
