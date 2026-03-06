using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Objects;
using RpgCompanion.Core.Options;
using RpgCompanion.Core.Selectors;

namespace RpgCompanion.DnD;

public record MeleeAttackEvent (
   IActor Attacker,
   ITarget Target,
   Dice Attempt,
   Weapon Weapon) 
   : IEvent<MeleeAttack>, IEvent<AttackAction>;

internal class MeleeAttack : IAction<MeleeAttackEvent>
{
   public MeleeAttackEvent For (IActor actor, Context context)
   {
      var currentPlayer = (IPlayer) null!;
      var targetOptions = IChoice<IObject>.Of(context => new OptionList<IObject>([]));
      var target = ITarget.Of(context => currentPlayer.ChooseFrom(targetOptions, context));
      return new MeleeAttackEvent(actor, target, new Dice.D20(), new Weapon());
   }
}