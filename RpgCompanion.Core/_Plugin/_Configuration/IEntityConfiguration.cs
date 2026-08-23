namespace RpgCompanion.Core;

public interface IEntityConfiguration<TEntity>
{
    void WithKey(string key);
    void WithCollection(string collectionName);
    void WithName(string name);
    void AddSubtype<TSubtype>() where TSubtype : TEntity;
}
