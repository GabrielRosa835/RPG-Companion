namespace RpgCompanion.Core;

public interface IEntity
{
    public DatabaseId DbId {get;}
}
public interface IEntity<TSelf> : IEntity where TSelf : class, IEntity<TSelf>
{
    public new DatabaseId<TSelf> DbId { get; }
}
