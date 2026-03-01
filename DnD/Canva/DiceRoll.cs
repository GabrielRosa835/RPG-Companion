using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.DnD;

public partial class DiceRoll : IRule<DiceRollEvent>
{
   public const string PENDING_DICE = nameof(PENDING_DICE);

   public DiceRollEvent Handle (IContext context)
   {
      Dice dice = context.SharedData[PENDING_DICE];
      return new DiceRollEvent(dice);
   }
}
public record DiceRollEvent (Dice Dice) : IEvent<DiceRoll>;

public class DiceRollHandler : IEventHandler<DiceRollEvent>
{
   public const string ROLL_RESULT = nameof(ROLL_RESULT);
   public void Handle (DiceRollEvent @event, IContext context)
   {
      context.SharedData[ROLL_RESULT] = @event.Dice.Roll();
      Console.WriteLine("Dice rolled: "+ context.SharedData[ROLL_RESULT]);
   }
}