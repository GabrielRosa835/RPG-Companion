using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.DnD.Canva;

internal record MeleeAttackAction(Attacker Attacker, Defender Defender) : AttackAction(Attacker), IEvent
{
   public class Effect : IEffect<MeleeAttackAction>
   {
      public bool ShouldApply (MeleeAttackAction attackEvent)
      {
         return true;
      }

      public void Apply (MeleeAttackAction action, IPipeline pipeline)
      {
         var weapon = action.Attacker.CurrentWeapon;
         var rollEvent = new DiceRoll(weapon.DamageDice);

         pipeline.Raise(rollEvent)
            .FollowedBy(rollEvent => new Damage(rollEvent.Result, action.Defender));
      }
   }
}