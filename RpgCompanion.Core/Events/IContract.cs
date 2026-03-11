using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IContract<TEvent> where TEvent : IEvent
{
    IEnumerable<ContextKey> Requirements { get; }
    IEnumerable<ContextKey> Outputs { get; }
}

public record EmptyContract<TEvent> : IContract<TEvent> where TEvent : IEvent
{
    public IEnumerable<ContextKey> Requirements => [];
    public IEnumerable<ContextKey> Outputs => [];
}