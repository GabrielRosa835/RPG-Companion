using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.DnD.Canva;

namespace RpgCompanion.DnD;

public class HealingAreaRule : IRule<MovementAction>
{
   public static readonly ContextKey<Defender> Target = new("Target");
   public static readonly ContextKey<dynamic> HealingSpell = new("Spell");

   public bool ShouldApply (IContext context)
   {
      Defender target = context.Data.Get(Target);
      return !target.IsDead;
   }

   public IEvent Apply (IContext context)
   {
      Defender target = context.Data.Get(Target);
      int amount = context.Data.Get(HealingSpell).HealingDice.Roll();
      return new Heal(amount, target);
   }
}