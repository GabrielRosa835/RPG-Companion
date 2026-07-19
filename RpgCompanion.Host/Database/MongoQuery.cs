namespace RpgCompanion.Host.Database;

using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Driver;

public class MongoQuery<T> : IQuery<T> where T : class, IEntity
{
    private readonly MongoDatabase _db;
    private readonly IMongoCollection<T> _collection;

    private FilterDefinition<T> _filter = Builders<T>.Filter.Empty;
    private SortDefinition<T>? _sort;
    private int? _skip;
    private int? _take;

    // A list of asynchronous actions to run AFTER the base entities are fetched
    private readonly List<Func<List<T>, Task>> _includeProcessors = new();

    internal MongoQuery(MongoDatabase db, IMongoCollection<T> collection)
    {
        _db = db;
        _collection = collection;
    }

    public IQuery<T> Filter(Expression<Func<T, bool>> predicate)
    {
        _filter = Builders<T>.Filter.And(_filter, Builders<T>.Filter.Where(predicate));
        return this;
    }

    public IQuery<T> Sort(Expression<Func<T, object>> keySelector, bool descending = false)
    {
        var sortBuilder = Builders<T>.Sort;
        _sort = descending ? sortBuilder.Descending(keySelector) : sortBuilder.Ascending(keySelector);
        return this;
    }

    public IQuery<T> Skip(int count)
    {
        _skip = count;
        return this;
    }

    public IQuery<T> Take(int count)
    {
        _take = count;
        return this;
    }

    public IQuery<T> Include<TIncluded>(Expression<Func<T, Rel<TIncluded>>> selector)
        where TIncluded : class, IEntity<TIncluded>
    {
        // 1. Extract PropertyInfo so we can write the "Loaded" state back later
        var propertyInfo = ExtractPropertyInfo(selector);

        // 2. Compile the expression so we can read the "Unloaded" state easily
        var getter = selector.Compile();

        // 3. Register the inclusion logic to run after execution
        _includeProcessors.Add(async (List<T> baseEntities) =>
        {
            var idsToFetch = new HashSet<DatabaseId<TIncluded>>();
            var mapping = new List<(T Entity, DatabaseId<TIncluded> Id)>();

            // Step A: Find all entities where this relationship is Unloaded
            foreach (var entity in baseEntities)
            {
                var relState = getter(entity);
                if (relState is Rel<TIncluded>.Unloaded unloaded)
                {
                    idsToFetch.Add(unloaded.DbId);
                    mapping.Add((entity, unloaded.DbId));
                }
            }

            if (idsToFetch.Count == 0) return;

            // Step B: Fetch the related entities in bulk
            var relatedCollection = _db.GetCollection<TIncluded>();
            var filter = Builders<TIncluded>.Filter.In(e => e.DbId, idsToFetch);
            var fetchedDocs = await relatedCollection.Find(filter).ToListAsync();

            // Create a dictionary for O(1) lookups
            var fetchedDict = fetchedDocs.ToDictionary(e => e.DbId);

            // Step C: Mutate the base entities, replacing Unloaded with Loaded
            foreach (var map in mapping)
            {
                if (fetchedDict.TryGetValue(map.Id, out var loadedRelatedEntity))
                {
                    var newRel = new Rel<TIncluded>.Loaded(loadedRelatedEntity);
                    propertyInfo.SetValue(map.Entity, newRel);
                }
            }
        });

        return this;
    }

    public async Task<List<T>> ExecuteAsync()
    {
        var query = _collection.Find(_filter);
        if (_sort != null) query = query.Sort(_sort);
        if (_skip.HasValue) query = query.Skip(_skip.Value);
        if (_take.HasValue) query = query.Limit(_take.Value);

        var results = await query.ToListAsync();

        // Execute all registered includes sequentially (or Task.WhenAll for parallel)
        foreach (var processor in _includeProcessors)
        {
            await processor(results);
        }

        return results;
    }

    /// <summary>
    /// Note: Includes are ignored in projections because projections
    /// return arbitrary types (DTOs), not the tracking entities.
    /// </summary>
    /// <param name="projection"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> projection)
    {
        var query = _collection.Find(_filter);
        if (_sort != null) query = query.Sort(_sort);
        if (_skip.HasValue) query = query.Skip(_skip.Value);
        if (_take.HasValue) query = query.Limit(_take.Value);

        return await query.Project(projection).ToListAsync();
    }

    // --- Helper Method ---
    private static PropertyInfo ExtractPropertyInfo<TSource, TProp>(Expression<Func<TSource, TProp>> expression)
    {
        return expression.Body is MemberExpression { Member: PropertyInfo propInfo }
            ? propInfo
            : throw new ArgumentException("Include selector must be a direct property access (e.g., x => x.Class).");
    }
}
