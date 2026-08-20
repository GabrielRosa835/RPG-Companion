namespace RpgCompanion.Canva;

using Core;

public class Entity : IEntity
{
    public DatabaseId DbId => Id;
    public required DatabaseId<Entity> Id { get; init; } = default!;
    public string TextValue { get; set; } = default!;
    public int NumberValue { get; set; }
    public ComplexValue ComplexValue { get; set; } = default!;
    public Rel<OtherEntity> RelationalValue { get; set; } = Rel.None<OtherEntity>();
}
