namespace RpgCompanion.Core;

using System.Numerics;

public interface ISerializationContext
{
    ISerializationContext String(string value);
    ISerializationContext Number<N>(N value) where N : INumber<N>;
    ISerializationContext Boolean(bool value);
    ISerializationContext Date(DateTime value);
    ISerializationContext Null();

    ISerializationContext Field(string name, Action<ISerializationContext> value);
    ISerializationContext Object(Action<ISerializationContext> value);
    ISerializationContext Array(Action<ISerializationContext> values);
    ISerializationContext Element<T>(T element);
}
