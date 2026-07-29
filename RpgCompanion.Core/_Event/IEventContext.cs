namespace RpgCompanion.Core;

using Toolbox;

public interface IEventContext : IEventTrigger
{
    /// <summary>
    /// Grants access to scoped dependencies for the current pipeline.
    /// </summary>
    IRegistry Registry { get; }

    IHostContext Host { get; }

    /// <summary>
    /// Grants access to contextual in-memory storage
    /// </summary>
    IStorage Storage { get; }

    /// <summary>
    /// The pipeline's CancellationToken
    /// </summary>
    CancellationToken CancellationToken { get; }

    /// <summary>
    /// Completely stops the pipeline by cancelling the token
    /// </summary>
    void Halt(bool throwException = false);

    /// <summary>
    /// Stops the pipeline, finishes executing the event methods and then return with the result
    /// </summary>
    /// <param name="result"></param>
    void Exit(EventResult result);

    void Continue(Event nextEvent);
    void Continue<TEvent>() where TEvent : Event;
}
