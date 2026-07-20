namespace RpgCompanion.Host.Database.Serializers;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

public class DatabaseIdSerializer : SerializerBase<DatabaseId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DatabaseId value)
    {
        context.Writer.WriteString(value.Value); // Save just the string
    }

    public override DatabaseId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new DatabaseId(context.Reader.ReadString()); // Read the string back into the struct
    }
}
