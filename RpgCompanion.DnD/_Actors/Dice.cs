namespace RpgCompanion.DnD;

public abstract record Dice
{
    public abstract int Roll();

    public abstract record Normal(int MaxValue) : Dice
    {
        public override int Roll() => Random.Shared.Next(1, MaxValue);
        public override string ToString() => $"D{MaxValue}";
    }

    public record Custom(int[] Faces) : Dice
    {
        public override int Roll() => Faces[Random.Shared.Next(Faces.Length)];
    }

    public record WithAdvantage(Dice Dice) : Dice
    {
        public override int Roll()
        {
            var first = Dice.Roll();
            var second = Dice.Roll();
            return first < second ? second : first;
        }
    }

    public record D4() : Normal(4);
    public record D6() : Normal(6);
    public record D8() : Normal(8);
    public record D10() : Normal(10);
    public record D12() : Normal(12);
    public record D20() : Normal(20);
}
