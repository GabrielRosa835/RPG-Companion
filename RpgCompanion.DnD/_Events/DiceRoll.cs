namespace RpgCompanion.DnD;

using Core;

public record DiceRoll(Dice Dice, int Modifier) : IEvent
{
    public int Result { get; private set; }

    public static Rule<DiceRoll> Apply = (DiceRoll e) =>
    {
        Console.WriteLine($"Realizando efeito de rolagem: \n   Dado: {e.Dice}");
        e.Result = e.Dice.Roll() + e.Modifier;
        Console.WriteLine($"Rolagem realizada com resultado: {e.Result}");
        return e;
    };
}
