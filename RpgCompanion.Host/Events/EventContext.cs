namespace RpgCompanion.Host.Events;

using Core;
using Toolbox;

public class EventContext(IRegistry _registry, IStorage _storage) : IEventContext
{
    internal EventExecutionContext ExecutionContext { get; set; } = default!;

    public IRegistry Registry => _registry;
    public IStorage Storage => _storage;
    public CancellationToken CancellationToken => ExecutionContext.CancellationSource.Token;

    public EventTask Raise(Event e, CancellationToken cancellationToken = default) =>
        ExecutionContext.Engine.Raise(e, cancellationToken);

    public void Halt() => ExecutionContext.CancellationSource?.Cancel();
}
