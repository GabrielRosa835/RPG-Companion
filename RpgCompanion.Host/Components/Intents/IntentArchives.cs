namespace RpgCompanion.Host;

using System.Collections.Concurrent;

internal class IntentArchives
{
    public ConcurrentDictionary<Type, IntentExecutor> Executors { get; } = new();
}
