using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.DnD;

public record Weapon(Dice DamageDice);
public record Attacker(Weapon CurrentWeapon);

public class Defender(int Health, bool IsDead)
{
    public int Health { get; set; } = Health;
    public bool IsDead { get; set; } = IsDead;
}

public class AttackAction(Weapon Weapon, Attacker Attacker, Defender Defender) : IEvent
{
    public static readonly ContextKey<Attacker> Attacker = new(nameof(Attacker));
    public static readonly ContextKey<Defender> Defender = new(nameof(Defender));
}

public class AttackActionEffect : IEffect<AttackAction>
{
    public bool ShouldApply(AttackAction attackEvent, IContext context)
    {
        var defender = context.Data.Get(AttackAction.Defender);
        return defender.Health > 0;
    }

    public void Apply(AttackAction action, IContext context)
    {
        var attacker = context.Data.Get(AttackAction.Attacker);
        var weapon = attacker.CurrentWeapon;
        var defender = context.Data.Get(AttackAction.Defender);
        var damageEvent = new Damage(weapon.DamageDice.Roll(), defender);
        context.Raise(damageEvent);
    }
}