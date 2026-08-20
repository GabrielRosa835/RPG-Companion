namespace RpgCompanion.Host;

internal class EntityConfiguration<TEntity>(
    PluginKey _pluginKey,
    EntityArchives _entityArchives,
    IServiceCollection _services)
    : IEntityConfiguration<TEntity> where TEntity : IEntity
{
    private EntityKey _key = new(Guid.CreateVersion7().ToString());
    private string? _collection;
    private string? _name;
    private readonly Dictionary<Type, Action> _subtypeRegistrations = new();

    public void Commit()
    {
        var descriptor = new EntityDescriptor
        {
            PluginKey = _pluginKey,
            Key = _key,
            Type = typeof(TEntity),
            Collection = _collection,
            Name = _name,
        };
        _services.AddKeyedSingleton(_key, descriptor);

        foreach (Action registration in _subtypeRegistrations.Values)
        {
            registration();
        }

        _entityArchives.Add(descriptor);
    }

    public void WithKey(string key)
    {
        _key = new EntityKey(key);
    }

    public void WithCollection(string collectionName)
    {
        _collection = collectionName;
    }

    public void WithName(string name)
    {
        _name = name;
    }

    public void AddSubtype<TSubtype>() where TSubtype : TEntity
    {
        // _subtypeRegistrations[typeof(TSubtype)] = () =>
        // {
        //     var cm = BsonClassMap.LookupClassMap(typeof(TEntity));
        //     cm.AddKnownType(typeof(TSubtype));
        // };
    }
}
