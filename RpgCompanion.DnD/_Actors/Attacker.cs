namespace RpgCompanion.DnD;

public record Attacker(string Name, Weapon? Weapon, int AttackModifier)
{
    public bool CanAttack => this.Weapon is not null;
}
