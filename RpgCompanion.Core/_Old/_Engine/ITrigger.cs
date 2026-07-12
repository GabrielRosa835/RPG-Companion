namespace RpgCompanion.Core;

public interface ITrigger
{
    EventTask Raise(Event e, CancellationToken? cancellationToken = null);
}
