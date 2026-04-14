namespace RpgCompanion.Core;

using Engine.Contexts;
using Events;

public record struct EmptyRule : IRule<EmptyEvent>
{
    public bool ShouldApply(IContext context) => true;
    public IEvent Apply(IContext context) => new EmptyEvent();
}
