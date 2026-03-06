using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IEventContract<TEvent> where TEvent : IEvent
{
    IEnumerable<ContextKey> Requirements { get; }
    IEnumerable<ContextKey> Outputs { get; }
}