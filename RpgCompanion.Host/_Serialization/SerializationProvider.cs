namespace RpgCompanion.Host;

using System.Text;
using System.Text.Json;
using Core;
using Microsoft.Extensions.Options;

public class SystemTextJsonSerializationProvider(
    IServiceProvider _serviceProvider,
    IOptions<SerializationOptions> options)
    : ISerializationProvider
{
    private readonly SerializationOptions _options = options.Value;

    public string Serialize<T>(T model)
    {
        var writerOptions = new JsonWriterOptions { Indented = _options.Indented };
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, writerOptions))
        {
            var context = new JsonSerializationContext(writer, _serviceProvider);
            var defaultSerializer = new DefaultSerializer(_serviceProvider);
            defaultSerializer.Serialize(model, typeof(T), context);
        }
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public T Deserialize<T>(string stringifiedModel)
    {
        using var document = JsonDocument.Parse(stringifiedModel);
        var context = new JsonDeserializationContext(_serviceProvider, document.RootElement);
        var defaultSerializer = new DefaultSerializer(_serviceProvider);
        return (T) defaultSerializer.Deserialize(typeof(T), context)!;
    }
}
