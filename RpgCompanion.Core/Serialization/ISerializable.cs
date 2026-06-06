namespace RpgCompanion.Core.Persistence;

public interface ISerializable<T>
{
    void Serialize(ISerializationContext context);
    static abstract T Deserialize(IDeserializationContext context);
}
