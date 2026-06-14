namespace RpgCompanion.Core;

public interface ISerializer<T>
{
    void Serialize(T model, ISerializationContext context);
    T Deserialize(IDeserializationContext context);
}
