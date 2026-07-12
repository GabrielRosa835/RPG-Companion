namespace RpgCompanion.Toolbox;

public abstract record Dice
{
   public abstract int Roll();

   #region Normal

   public abstract record Normal(int MaxValue) : Dice
   {
      public override int Roll() => Random.Shared.Next(1, MaxValue);
      public override string ToString() => $"D{MaxValue}";
   }

   public record D3() : Normal(3);

   public record D4() : Normal(4);

   public record D6() : Normal(6);

   public record D8() : Normal(8);

   public record D10() : Normal(10);

   public record D12() : Normal(12);

   public record D20() : Normal(20);

   #endregion

   #region Custom

   public record Custom(int[] Faces) : Dice
   {
      public override int Roll() => Faces[Random.Shared.Next(Faces.Length)];
      public override string ToString() => $"D{Faces.Length}[{string.Join(',', Faces)}]";
   }

   #endregion

   #region Composites

   public record WithAdvantage(Dice Dice) : Dice
   {
      public override int Roll() => new WithNAdvantage(2, Dice).Roll();
   }

   public record WithNAdvantage(int N, Dice Dice) : Dice
   {
      public override int Roll() => Enumerable.Repeat(Dice, N).Select(d => d.Roll()).Max();
   }

   public record WithDisadvantage(Dice Dice) : Dice
   {
      public override int Roll() => new WithNDisadvantage(2, Dice).Roll();
   }

   public record WithNDisadvantage(int N, Dice Dice) : Dice
   {
      public override int Roll() => Enumerable.Repeat(Dice, N).Select(d => d.Roll()).Min();
   }

   public record Group(int Amount, Dice Dice) : Dice
   {
      public override int Roll() => Enumerable.Repeat(Dice, Amount).Select(d => d.Roll()).Sum();
   }

   #endregion

   public record Sequence(int Amount, Dice Dice)
   {
      public int[] Roll() => Enumerable.Repeat(Dice, Amount).Select(d => d.Roll()).ToArray();
   }
}
