using RpgCompanion.Core.Objects;

namespace RpgCompanion.DnD;

public abstract class Dice : IComponent
{
   public abstract int Roll ();

   public class D20 : NormalDice
   {
      public override int MaxValue => 20;
   }
   public class D4 : NormalDice
   {
      public override int MaxValue => 4;
   }

   public abstract class NormalDice : Dice
   {
      public abstract int MaxValue { get; }
      public override int Roll ()
      {
         return Random.Shared.Next(1, MaxValue + 1);
      }
   }
}
