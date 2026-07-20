namespace RpgCompanion.Core;

public interface IEventTrigger
{
    EventTask Raise(Event e, CancellationToken cancellationToken = default);
}
