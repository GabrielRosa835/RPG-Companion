namespace RpgCompanion.Host.Serialization;

using System.Text;
using System.Text.Json;
using Core.Persistence;

public interface ISerializationProvider
{
    string Serialize<T>(T model) where T : ISerializable<T>;
    T Deserialize<T>(string json) where T : ISerializable<T>;
}


public class SystemTextJsonSerializationProvider : ISerializationProvider
{
    private readonly JsonWriterOptions _writerOptions;

    public SystemTextJsonSerializationProvider(bool indented = false)
    {
        _writerOptions = new JsonWriterOptions { Indented = indented };
    }

    public string Serialize<T>(T model) where T : ISerializable<T>
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, _writerOptions))
        {
            var context = new JsonSerializationContext(writer);
            model.Serialize(context);
        } // The writer must be disposed/flushed before reading the stream

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public T Deserialize<T>(string json) where T : ISerializable<T>
    {
        using var document = JsonDocument.Parse(json);
        var context = new JsonDeserializationContext(document.RootElement);

        return T.Deserialize(context);
    }
}
