namespace RpgCompanion.Host;

using System.Numerics;
using System.Text.Json;
using Core;

public class JsonSerializationContext(
    Utf8JsonWriter _writer,
    IServiceProvider _serviceProvider)
    : ISerializationContext
{
    private ISerializationContext Write(Action writer)
    {
        writer();
        return this;
    }

    public ISerializationContext Field(string name, Action<ISerializationContext> value)
    {
        _writer.WritePropertyName(name);
        value(this);
        return this;
    }

    public ISerializationContext String(string value) => Write(() => _writer.WriteStringValue(value));

    public ISerializationContext Number<N>(N value) where N : INumber<N>
    {
        var type = typeof(N);

        if (type == typeof(int)) return Write(() => _writer.WriteNumberValue(int.CreateChecked(value)));
        if (type == typeof(float)) return Write(() => _writer.WriteNumberValue(float.CreateChecked(value)));
        if (type == typeof(double)) return Write(() => _writer.WriteNumberValue(double.CreateChecked(value)));
        if (type == typeof(decimal)) return Write(() => _writer.WriteNumberValue(decimal.CreateChecked(value)));
        if (type == typeof(long)) return Write(() => _writer.WriteNumberValue(long.CreateChecked(value)));
        if (type == typeof(uint)) return Write(() => _writer.WriteNumberValue(uint.CreateChecked(value)));
        if (type == typeof(ulong)) return Write(() => _writer.WriteNumberValue(ulong.CreateChecked(value)));
        if (type == typeof(short)) return Write(() => _writer.WriteNumberValue(short.CreateChecked(value)));
        if (type == typeof(ushort)) return Write(() => _writer.WriteNumberValue(ushort.CreateChecked(value)));
        if (type == typeof(byte)) return Write(() => _writer.WriteNumberValue(byte.CreateChecked(value)));
        if (type == typeof(sbyte)) return Write(() => _writer.WriteNumberValue(sbyte.CreateChecked(value)));

        throw SerializationExceptions.UnsupportedNumberException("Value");
    }

    public ISerializationContext Boolean(bool value) => Write(() => _writer.WriteBooleanValue(value));
    public ISerializationContext Null() => Write(_writer.WriteNullValue);

    public ISerializationContext Object(Action<ISerializationContext> nesting)
    {
        _writer.WriteStartObject();
        nesting(this);
        _writer.WriteEndObject();
        return this;
    }

    public ISerializationContext Array(Action<ISerializationContext> nesting)
    {
        _writer.WriteStartArray();
        nesting(this);
        _writer.WriteEndArray();
        return this;
    }

    public ISerializationContext Element<TElement>(TElement element)
    {
        var serializer = _serviceProvider.GetService<ISerializer<TElement>>();
        if (serializer is not null)
        {
            serializer.Serialize(element, this);
            return this;
        }
        var defaultSerializer = new DefaultSerializer(_serviceProvider);
        defaultSerializer.Serialize(element, typeof(TElement), this);
        return this;
    }
}
