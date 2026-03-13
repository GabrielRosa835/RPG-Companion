using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IContract<TEvent> where TEvent : IEvent
{
    IEnumerable<ContextKey> Requirements { get; }
    IEnumerable<ContextKey> Outputs { get; }
}