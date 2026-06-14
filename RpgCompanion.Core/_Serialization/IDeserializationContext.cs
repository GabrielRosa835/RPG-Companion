namespace RpgCompanion.Core;

using System.Numerics;

public interface IDeserializationContext
{
    bool IsNull();

    string GetString();
    bool TryGetString(out string value);

    N GetNumber<N>() where N : INumber<N>;
    bool TryGetNumber<N>(out N value) where N : INumber<N>;

    bool GetBoolean();
    bool TryGetBoolean(out bool value);

    DateTime GetDate();
    bool TryGetDate(out DateTime value);

    TValue GetField<TValue>(string fieldName, Func<IDeserializationContext, TValue> factory);
    bool TryGetField<TValue>(string fieldName, Func<IDeserializationContext, TValue> factory, out TValue value);

    TModel GetObject<TModel>(Func<IDeserializationContext, TModel> factory);
    bool TryGetObject<TModel>(Func<IDeserializationContext, TModel> factory, out TModel value);

    IEnumerable<TElement> GetArray<TElement>(Func<IDeserializationContext, TElement> factory);
    bool TryGetArray<TElement>(Func<IDeserializationContext, TElement> factory, out IEnumerable<TElement> value);

    /// <summary>
    /// Constructs a generic element via an external Serializer
    /// </summary>
    TValue Get<TValue>();
    bool TryGet<TValue>(out TValue value);
}
