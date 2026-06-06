namespace RpgCompanion.DnD;

using Core;
using Core.Toolbox;

public record Weapon
{
    public Dice.D6 DamageDice { get; set; } = default!;
}
