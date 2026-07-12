namespace RpgCompanion.DnD._Old._Actors;

using RpgCompanion.Toolbox;

public record Weapon
{
    public Dice.D6 DamageDice { get; set; } = default!;
}
