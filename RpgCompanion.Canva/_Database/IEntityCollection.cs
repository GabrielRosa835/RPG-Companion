namespace RpgCompanion.Core;

public interface ICollection
{
    void Remove(object subject);
    void Save(object subject, string id);
    object Find()
}

public interface IEntityCollection<TEntity> where TEntity : IEntity
{

}
