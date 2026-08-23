namespace RpgCompanion.Core;

public interface IEventTrigger
{
    /// <summary>
    /// Starts a new event pipeline and returns the pipeline's task
    /// </summary>
    Task<EventResult> Raise(Event e, CancellationToken cancellationToken = default);
}
