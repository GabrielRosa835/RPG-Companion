namespace RpgCompanion.Core.Persistence;

public interface ISerializator<T>
{
    T Deserialize(IDeserializationContext context);
    void Serialize(ISerializationContext context);
}
