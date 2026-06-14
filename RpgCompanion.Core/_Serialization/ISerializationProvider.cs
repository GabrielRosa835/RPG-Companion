namespace RpgCompanion.Core;

public interface ISerializationProvider
{
    string Serialize<T>(T model);
    T Deserialize<T>(string stringifiedModel);
}
