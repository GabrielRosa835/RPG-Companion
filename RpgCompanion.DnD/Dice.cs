namespace RpgCompanion.DnD.Canva;

public abstract class Dice
{
    public abstract int Roll();

    public abstract class NormalDice(int MaxValue) : Dice
    {
        public override int Roll() => Random.Shared.Next(1, MaxValue);
        public override string ToString() => $"D{MaxValue}";
    }
}

public class D20() : Dice.NormalDice(20);
public class D6() : Dice.NormalDice(6);
