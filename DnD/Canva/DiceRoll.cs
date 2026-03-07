using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.DnD;

public record DiceRoll(Dice Dice) : EventBase(nameof(DiceRoll)), IEventContract<DiceRoll>
{
   public static readonly ContextKey<Dice> PendingDice = new(nameof(PendingDice));
   public static readonly ContextKey<int> RollResult = new(nameof(RollResult));
   
   public IEnumerable<ContextKey> Requirements => [PendingDice];
   public IEnumerable<ContextKey> Outputs => [RollResult];
}

public class DiceRollRule : IRule<DiceRoll>
{
   public DiceRoll Apply(IContext context)
   {
      return new DiceRoll(new Dice.D20());
   }
   public bool ShouldApply (IContext context) => true;
}

public class DiceRollEffect : IEffect<DiceRoll>
{
   public void Apply(DiceRoll @event, IContext context)
   {
      var dice = context.Data.Get(DiceRoll.PendingDice);
      int result = dice.Roll();
      context.Data.Set(DiceRoll.RollResult, result);
   }
   public bool ShouldApply (DiceRoll @event, IContext context) => true;
}