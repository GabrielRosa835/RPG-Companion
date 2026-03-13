using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;

namespace RpgCompanion.DnD;

public record Damage(int Value, Defender Target) : IEvent;

public class DamageEffect : IEffect<Damage>
{
    public bool ShouldApply(Damage damageEvent, IContext context)
    {
        return damageEvent.Target.Health > damageEvent.Value;
    }

    public void Apply(Damage damageEvent, IContext context)
    {
        damageEvent.Target.Health -= damageEvent.Value;
        if (damageEvent.Target.Health <= 0)
        {
            damageEvent.Target.Health = 0;
        }
    }
}