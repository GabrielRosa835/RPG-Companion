namespace RpgCompanion.Host;

using System.Numerics;
using System.Runtime.Serialization;
using System.Text.Json;
using Core;

public class JsonDeserializationContext(
    IServiceProvider _serviceProvider,
    JsonElement _element) : IDeserializationContext
{
    public bool IsNull() => _element.ValueKind == JsonValueKind.Null;

    public string GetString()
    {
        if (_element.ValueKind != JsonValueKind.String)
        {
            throw new SerializationException("Element is not a string.");
        }
        return _element.GetString()!;
    }

    public bool TryGetString(out string value)
    {
        if (_element.ValueKind == JsonValueKind.String)
        {
            value = _element.GetString()!;
            return true;
        }
        value = string.Empty;
        return false;
    }

    public N GetNumber<N>() where N : INumber<N>
    {
        if (_element.ValueKind != JsonValueKind.Number)
        {
            throw new SerializationException($"Cannot deserialize {typeof(N).Name}: Element is not a number.");
        }

        var type = typeof(N);

        if (type == typeof(byte)) return N.CreateChecked(_element.GetByte());
        if (type == typeof(sbyte)) return N.CreateChecked(_element.GetSByte());
        if (type == typeof(short)) return N.CreateChecked(_element.GetInt16());
        if (type == typeof(ushort)) return N.CreateChecked(_element.GetUInt16());
        if (type == typeof(int)) return N.CreateChecked(_element.GetInt32());
        if (type == typeof(uint)) return N.CreateChecked(_element.GetUInt32());
        if (type == typeof(long)) return N.CreateChecked(_element.GetInt64());
        if (type == typeof(ulong)) return N.CreateChecked(_element.GetUInt64());
        if (type == typeof(float)) return N.CreateChecked(_element.GetSingle());
        if (type == typeof(double)) return N.CreateChecked(_element.GetDouble());
        if (type == typeof(decimal)) return N.CreateChecked(_element.GetDecimal());

        throw SerializationExceptions.UnsupportedNumberException("Element");
    }

    public bool TryGetNumber<N>(out N value) where N : INumber<N>
    {
        if (_element.ValueKind == JsonValueKind.Number)
        {
            try
            {
                value = GetNumber<N>();
                return true;
            }
            catch { /* ignored */ }
        }
        value = default!;
        return false;
    }

    public bool GetBoolean()
    {
        if (_element.ValueKind == JsonValueKind.True) return true;
        if (_element.ValueKind == JsonValueKind.False) return false;
        throw new SerializationException("Element is not a boolean.");
    }

    public bool TryGetBoolean(out bool value)
    {
        if (_element.ValueKind == JsonValueKind.True)
        {
            value = true;
            return true;
        }
        if (_element.ValueKind == JsonValueKind.False)
        {
            value = false;
            return true;
        }
        value = default;
        return false;
    }

    public TValue GetField<TValue>(string fieldName, Func<IDeserializationContext, TValue> factory)
    {
        if (_element.ValueKind != JsonValueKind.Object)
        {
            throw new SerializationException($"Cannot get field '{fieldName}': Element is not an object.");
        }
        if (!_element.TryGetProperty(fieldName, out var prop))
        {
            throw new SerializationException($"Field '{fieldName}' not found on the JSON object.");
        }

        var nestedContext = new JsonDeserializationContext(_serviceProvider, prop);
        return factory(nestedContext);
    }

    public bool TryGetField<TValue>(string fieldName, Func<IDeserializationContext, TValue> factory, out TValue value)
    {
        if (_element.ValueKind == JsonValueKind.Object && _element.TryGetProperty(fieldName, out var prop))
        {
            try
            {
                var nestedContext = new JsonDeserializationContext(_serviceProvider, prop);
                value = factory(nestedContext);
                return true;
            }
            catch { /* ignored */ }
        }

        value = default!;
        return false;
    }

    public TModel GetObject<TModel>(Func<IDeserializationContext, TModel> factory)
    {
        if (_element.ValueKind != JsonValueKind.Object)
        {
            throw new SerializationException($"Cannot deserialize {typeof(TModel).Name}: Element is not an object.");
        }

        var nestedContext = new JsonDeserializationContext(_serviceProvider, _element);
        return factory(nestedContext);
    }

    public bool TryGetObject<TModel>(Func<IDeserializationContext, TModel> factory, out TModel value)
    {
        if (_element.ValueKind == JsonValueKind.Object)
        {
            try
            {
                var nestedContext = new JsonDeserializationContext(_serviceProvider, _element);
                value = factory(nestedContext);
                return true;
            }
            catch { /* ignored */ }
        }
        value = default!;
        return false;
    }

    public IEnumerable<TElement> GetArray<TElement>(Func<IDeserializationContext, TElement> factory)
    {
        if (_element.ValueKind != JsonValueKind.Array)
        {
            throw new SerializationException("Element is not an array.");
        }

        foreach (var item in _element.EnumerateArray())
        {
            var itemContext = new JsonDeserializationContext(_serviceProvider, item);
            yield return factory(itemContext);
        }
    }

    public bool TryGetArray<TElement>(Func<IDeserializationContext, TElement> factory, out IEnumerable<TElement> value)
    {
        if (_element.ValueKind == JsonValueKind.Array)
        {
            try
            {
                // Materialize the array immediately so errors within the factory are
                // caught within the try/catch, rather than deferred via yield return.
                var results = new List<TElement>();
                foreach (var item in _element.EnumerateArray())
                {
                    var itemContext = new JsonDeserializationContext(_serviceProvider, item);
                    results.Add(factory(itemContext));
                }

                value = results;
                return true;
            }
            catch { /* ignored */ }
        }

        value = default!;
        return false;
    }

    /// <summary>
    /// Constructs a generic element via an external Serializer
    /// </summary>
    public TValue Get<TValue>()
    {
        var defaultSerializer = new DefaultSerializer(_serviceProvider);
        return (TValue) defaultSerializer.Deserialize(typeof(TValue), this)!;
    }

    public bool TryGet<TValue>(out TValue value)
    {
        try
        {
            var defaultSerializer = new DefaultSerializer(_serviceProvider);
            value = (TValue) defaultSerializer.Deserialize(typeof(TValue), this)!;
            return true;
        }
        catch { /* ignored */ }

        value = default!;
        return false;
    }
}
