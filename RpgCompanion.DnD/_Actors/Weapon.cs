namespace RpgCompanion.DnD;

using Core;

public record Weapon
{
    public Dice.D6 DamageDice { get; set; } = default!;
}
