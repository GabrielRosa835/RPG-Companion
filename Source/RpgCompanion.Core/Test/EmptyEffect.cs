namespace RpgCompanion.Core;

using Engine;
using Events;

public record struct EmptyEffect : IEffect<EmptyEvent>
{
    public bool ShouldApply(EmptyEvent e) => true;
    public void Apply(EmptyEvent e, IPipeline pipeline) { }
}
