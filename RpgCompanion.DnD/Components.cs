namespace RpgCompanion.DnD.Canva;

public record Weapon(Dice DamageDice, int DamageModifier);

public record Attacker(string Name, Weapon? Weapon, int AttackModifier)
{
    public bool CanAttack => this.Weapon is not null;
}

public record Defender(string Name, int Health, int Defense)
{
    public int Health { get; set; } = Health;
}
