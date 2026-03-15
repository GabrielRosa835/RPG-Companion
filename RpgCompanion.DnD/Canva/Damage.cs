namespace RpgCompanion.DnD;

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.DnD.Canva;

public record Damage(int Value, Defender Target) : IEvent;

public class DamageEffect : IEffect<Damage>
{
    public bool ShouldApply(Damage e)
    {
        return e.Target.Health > e.Value;
    }

    public void Apply(Damage e, IPipeline pipeline)
    {
        e.Target.Health -= e.Value;
        if (e.Target.Health <= 0)
        {
            e.Target.Health = 0;
        }
    }
}
