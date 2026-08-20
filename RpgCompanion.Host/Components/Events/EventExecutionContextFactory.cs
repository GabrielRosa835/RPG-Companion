namespace RpgCompanion.Host;

internal class EventExecutionContextFactory(
    EventEngine engine,
    IEventFactory eventFactory,
    EventContextFactory contextFactory,
    ScopeProvider scopeProvider)
{
    internal EventExecutionContext Create(CancellationToken ct)
    {
        EventExecutionContext context = default!;
        context = new EventExecutionContext
        {
            Engine = engine,
            Factory = eventFactory,
            CancellationSource = CancellationTokenSource.CreateLinkedTokenSource(ct),
            Context = contextFactory.Create(context),
            ServiceScope = scopeProvider.CreateScope(),
        };
        return context;
    }
}
