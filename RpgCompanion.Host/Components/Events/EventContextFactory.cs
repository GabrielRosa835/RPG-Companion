namespace RpgCompanion.Host;

using Toolbox.Storage;

internal class EventContextFactory(IRegistry registry)
{
    internal EventContext Create(EventExecutionContext executionContext)
    {
        return new EventContext
        {
            Storage = new ConcurrentDynamicStorage(),
            Registry = registry,
            ExecutionContext = executionContext,
        };
    }
}
