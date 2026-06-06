namespace RpgCompanion.Core.Persistence;

using System.Numerics;

public interface ISerializationContext
{
    // Chaining methods for writing
    ISerializationContext Field(string name);
    ISerializationContext String(string value);
    ISerializationContext Number<N>(N value) where N : INumber<N>;
    ISerializationContext Boolean(bool value);
    ISerializationContext Null();

    // Scoped delegate methods replace Start/End mechanics
    ISerializationContext Object(Action<ISerializationContext> nesting);
    ISerializationContext Array(Action<ISerializationContext> nesting);
}
