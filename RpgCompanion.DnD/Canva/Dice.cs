namespace RpgCompanion.DnD;

public abstract record Dice
{
   public abstract int Roll ();

   public record D20 () : Normal(20);
   public record D4 () : Normal(4);

   public abstract record Normal(int MaxValue) : Dice
   {
      public override int Roll ()
      {
         return Random.Shared.Next(1, MaxValue + 1);
      }
   }
}
