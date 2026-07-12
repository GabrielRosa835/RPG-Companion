namespace RpgCompanion.DnD._Old._Actors;

public record Enemy
{
    public string Name { get; set; } = default!;
    public int Health { get; set; }
    public int AC { get; set; }
}
