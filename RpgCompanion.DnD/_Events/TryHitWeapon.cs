namespace RpgCompanion.DnD;

using Core;

public record TryHitWeapon(int Modifier, int TargetValue) : IEvent
{
    public bool Hit { get; private set; }

    public static Rule<TryHitWeapon> Apply => (e) =>
    {
        Console.WriteLine(
            $"Realizando efeito de tentativa de ataque: Target = {e.TargetValue}, Modifier = {e.Modifier}");
        int result = new Dice.D20().Roll() + e.Modifier;
        Console.WriteLine($"Rolagem realizada com resultado: {result}");
        var hit = result >= e.TargetValue;
        e.Hit = hit;
        Console.WriteLine($"Sucesso: {hit}");
        return e;
    };
}
