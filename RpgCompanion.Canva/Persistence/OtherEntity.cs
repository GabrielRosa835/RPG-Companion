namespace RpgCompanion.Canva;

using Core;

public class OtherEntity : IEntity<OtherEntity>
{
    public DatabaseId<OtherEntity> DbId { get; init; }
    public string TextValue { get; set; } = default!;
    public int NumberValue { get; set; }
}
