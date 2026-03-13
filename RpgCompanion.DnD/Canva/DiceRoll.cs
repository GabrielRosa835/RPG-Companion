using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.DnD;

public record DiceRoll(Dice Dice, IEvent Next) : IEvent;

public class DiceRollEffect : IEffect<DiceRoll>
{
   public void Apply(DiceRoll diceRollEvent, IContext context)
   {
      int result = diceRollEvent.Dice.Roll();
      context.Raise(diceRollEvent);
   }
   public bool ShouldApply (DiceRoll @event, IContext context) => true;
}