using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.DnD;

public record DiceRoll(Dice Dice) : IEvent
{
   public int Result { get; private set; } = 0;

   public class Effect : IEffect<DiceRoll>
   {
      public void Apply (DiceRoll diceRollEvent, IPipeline pipeline)
      {
         int result = diceRollEvent.Dice.Roll();
         diceRollEvent.Result = result;
      }
      public bool ShouldApply (DiceRoll @event) => true;
   }
}

