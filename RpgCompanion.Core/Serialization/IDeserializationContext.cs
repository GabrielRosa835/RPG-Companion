namespace RpgCompanion.Core.Persistence;

using System.Numerics;

public interface IDeserializationContext
{
    string GetString(string? fieldName = null);
    N GetNumber<N>(string? fieldName = null) where N : INumber<N>;
    bool GetBoolean(string? fieldName = null);
    bool IsNull(string? fieldName = null);

    // Scoped retrieval using Func to return the constructed nested objects
    TModel GetObject<TModel>(string? fieldName, Func<IDeserializationContext, TModel> factory);
    IEnumerable<TElement> GetArray<TElement>(string? fieldName, Func<IDeserializationContext, TElement> factory);
}
