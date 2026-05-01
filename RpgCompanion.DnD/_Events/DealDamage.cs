namespace RpgCompanion.DnD;

using Core;

public record DealDamage(Defender Defender, int Damage) : IEvent
{
    public static Rule<DealDamage, bool> ShouldApply = (e) => e.Damage > 0;

    public static Rule<DealDamage> Apply => (e) =>
    {
        Console.WriteLine(
            $"Realizando efeito de dano: \n   Defensor: {e.Defender.Name} ({e.Defender.Health}HP) \n   Dano: {e.Damage}");
        e.Defender.Health -= e.Damage;
        if (e.Defender.Health <= 0)
        {
            e.Defender.Health = 0;
        }
        Console.WriteLine($"Vida após dano: {e.Defender.Health}HP");
        return e;
    };
}
