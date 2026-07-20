namespace RpgCompanion.Host.Database;

using MongoDB.Bson.Serialization;
using Serializers;

public class AppSerializationProvider : IBsonSerializationProvider
{
    public IBsonSerializer? GetSerializer(Type type)
    {
        // Handle non-generic DatabaseId
        if (type == typeof(DatabaseId)) return new DatabaseIdSerializer();

        if (!type.IsGenericType) return null;

        var genericTypeDefinition = type.GetGenericTypeDefinition();

        if (genericTypeDefinition == typeof(DatabaseId<>))
        {
            var entityType = type.GetGenericArguments()[0];
            var serializerType = typeof(TypedDatabaseIdSerializer<>).MakeGenericType(entityType);
            return (IBsonSerializer)Activator.CreateInstance(serializerType)!;
        }
        if (genericTypeDefinition == typeof(Rel<>))
        {
            var entityType = type.GetGenericArguments()[0];
            var serializerType = typeof(RelSerializer<>).MakeGenericType(entityType);
            return (IBsonSerializer)Activator.CreateInstance(serializerType)!;
        }

        return null;
    }
}
