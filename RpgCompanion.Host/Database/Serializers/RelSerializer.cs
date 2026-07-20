namespace RpgCompanion.Host.Database;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

public class RelSerializer<T> : SerializerBase<Rel<T>> where T : class, IEntity
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Rel<T> value)
    {
        var idValue = value switch
        {
            Rel<T>.Loaded loaded => loaded.Entity.DbId.Value,
            Rel<T>.Unloaded unloaded => unloaded.DbId.Value,
            Rel<T>.None none => string.Empty,
            _ => throw new InvalidOperationException("Unknown Rel state")
        };
        context.Writer.WriteString(idValue);
    }

    public override Rel<T> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        // Always deserialize as Unloaded. The application must explicitly fetch the entity if needed.
        var idString = context.Reader.ReadString();
        return new Rel<T>.Unloaded(new DatabaseId<T>(idString));
    }
}
