namespace RpgCompanion.Canva;

using Core;

public class Entity : IEntity<Entity>
{
    public DatabaseId<Entity> DbId { get; init; }
    public string TextValue { get; set; } = default!;
    public int NumberValue { get; set; }
    public ComplexValue ComplexValue { get; set; } = default!;
    public Rel<OtherEntity> RelationalValue { get; set; } = Rel.None<OtherEntity>();
}
