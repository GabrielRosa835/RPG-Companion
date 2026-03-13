using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.DnD;

public record Heal(int Value, Defender target) : IEvent;

public class HealingAreaRule : IRule
{
    public static readonly ContextKey<Defender> Target = new("Target");
    public static readonly ContextKey<dynamic> HealingSpell = new("Spell");
    
    public bool ShouldApply(ISnapshot context)
    {
        Defender target = context.Data.Get(Target);
        return !target.IsDead;
    }

    public IEvent Apply(ISnapshot context)
    {
        Defender target = context.Data.Get(Target);
        int amount = context.Data.Get(HealingSpell).HealingDice.Roll();
        return new Heal(amount, target);
    }
}