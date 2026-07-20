namespace RpgCompanion.Host.Database;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

public class TypedDatabaseIdSerializer<T> : SerializerBase<DatabaseId<T>> where T : class, IEntity
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DatabaseId<T> value)
    {
        context.Writer.WriteString(value.Value);
    }

    public override DatabaseId<T> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new DatabaseId<T>(context.Reader.ReadString());
    }
}
