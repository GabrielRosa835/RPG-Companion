namespace RpgCompanion.Host.Serialization;

using System.Numerics;
using System.Text.Json;
using Core.Persistence;

public class JsonSerializationContext : ISerializationContext
{
    private readonly Utf8JsonWriter _writer;
    private string? _currentField;

    public JsonSerializationContext(Utf8JsonWriter writer)
    {
        _writer = writer;
    }

    public ISerializationContext Field(string name)
    {
        _currentField = name;
        return this;
    }

    public ISerializationContext String(string value)
    {
        if (_currentField != null)
        {
            _writer.WriteString(_currentField, value);
            _currentField = null;
        }
        else
        {
            _writer.WriteStringValue(value);
        }
        return this;
    }

    public ISerializationContext Number<N>(N value) where N : INumber<N>
    {
        // Safe fallback for generic numbers into STJ
        var decimalValue = Convert.ToDecimal(value);

        if (_currentField != null)
        {
            _writer.WriteNumber(_currentField, decimalValue);
            _currentField = null;
        }
        else
        {
            _writer.WriteNumberValue(decimalValue);
        }
        return this;
    }

    public ISerializationContext Boolean(bool value)
    {
        if (_currentField != null)
        {
            _writer.WriteBoolean(_currentField, value);
            _currentField = null;
        }
        else
        {
            _writer.WriteBooleanValue(value);
        }
        return this;
    }

    public ISerializationContext Null()
    {
        if (_currentField != null)
        {
            _writer.WriteNull(_currentField);
            _currentField = null;
        }
        else
        {
            _writer.WriteNullValue();
        }
        return this;
    }

    public ISerializationContext Object(Action<ISerializationContext> nesting)
    {
        if (_currentField != null)
        {
            _writer.WriteStartObject(_currentField);
            _currentField = null;
        }
        else
        {
            _writer.WriteStartObject();
        }

        nesting(this);
        _writer.WriteEndObject();
        return this;
    }

    public ISerializationContext Array(Action<ISerializationContext> nesting)
    {
        if (_currentField != null)
        {
            _writer.WriteStartArray(_currentField);
            _currentField = null;
        }
        else
        {
            _writer.WriteStartArray();
        }

        nesting(this);
        _writer.WriteEndArray();
        return this;
    }
}
