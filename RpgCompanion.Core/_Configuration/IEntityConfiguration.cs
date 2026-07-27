namespace RpgCompanion.Core;

public interface IEntityConfiguration<TEntity> where TEntity : IEntity
{
    void WithKey(EntityKey key);
    void WithCollection(string collectionName);
    void WithName(string name);
    void AddSubtype<TSubtype>() where TSubtype : TEntity;
}
