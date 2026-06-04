namespace RpgCompanion.DnD;

public record Enemy
{
    public string Name { get; set; } = default!;
    public int Health { get; set; }
    public int AC { get; set; }
}
