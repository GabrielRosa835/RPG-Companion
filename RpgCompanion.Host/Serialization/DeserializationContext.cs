namespace RpgCompanion.Host.Serialization;

using System.Numerics;
using System.Text.Json;
using Core.Persistence;

public class JsonDeserializationContext : IDeserializationContext
{
    private readonly JsonElement _element;

    public JsonDeserializationContext(JsonElement element)
    {
        _element = element;
    }

    private JsonElement GetTargetElement(string? fieldName)
    {
        if (string.IsNullOrEmpty(fieldName)) return _element;
        return _element.TryGetProperty(fieldName, out var prop) ? prop : default;
    }

    public string GetString(string? fieldName = null)
    {
        return GetTargetElement(fieldName).GetString() ?? string.Empty;
    }

    public N GetNumber<N>(string? fieldName = null) where N : INumber<N>
    {
        var target = GetTargetElement(fieldName);
        if (target.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return N.Zero;
        }

        // Using INumber.Parse against the raw JSON string is the safest cross-type conversion
        return N.Parse(target.ToString(), null);
    }

    public bool GetBoolean(string? fieldName = null)
    {
        var target = GetTargetElement(fieldName);
        return target.ValueKind == JsonValueKind.True;
    }

    public bool IsNull(string? fieldName = null)
    {
        return GetTargetElement(fieldName).ValueKind == JsonValueKind.Null;
    }

    public TModel GetObject<TModel>(string? fieldName, Func<IDeserializationContext, TModel> factory)
    {
        var target = GetTargetElement(fieldName);
        if (target.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException($"Cannot deserialize {typeof(TModel).Name}: Element is not an object.");
        }

        var nestedContext = new JsonDeserializationContext(target);
        return factory(nestedContext);
    }

    public IEnumerable<TElement> GetArray<TElement>(string? fieldName, Func<IDeserializationContext, TElement> factory)
    {
        var target = GetTargetElement(fieldName);
        if (target.ValueKind != JsonValueKind.Array)
        {
            yield break;
        }

        foreach (var item in target.EnumerateArray())
        {
            var itemContext = new JsonDeserializationContext(item);
            yield return factory(itemContext);
        }
    }
}
