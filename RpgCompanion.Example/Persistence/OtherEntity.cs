namespace RpgCompanion.Canva;

using Core;

public class OtherEntity : IEntity
{
    public required DatabaseId<OtherEntity> Id { get; init; }
    public DatabaseId DbId => Id;
    public string TextValue { get; set; } = default!;
    public int NumberValue { get; set; }
}
