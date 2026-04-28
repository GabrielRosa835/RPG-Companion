namespace RpgCompanion.DnD;

public record Defender(string Name, int Health, int Defense)
{
    public int Health { get; set; } = Health;
}
