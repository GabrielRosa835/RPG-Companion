namespace RpgCompanion.Host;

using System.Runtime.Serialization;

public static class SerializationExceptions
{
    public static Exception UnsupportedNumberException(string paramName)
    {
        const string types = "byte, sbyte, short, ushort, int, uint, long, ulong, float, double, decimal and their counterparts";
        var inner = new NotSupportedException($"Only .NET native numbers are allowed for now: {types}");
        throw new SerializationException($"{paramName} is not a valid number.", inner);
    }
    public static Exception ParameterlessConstructorRequiredException(Type type)
    {
        var inner = new InvalidOperationException($"Cannot create an instance of {type.Name}, no parameterless constructor was found.");
        return new SerializationException($"Cannot deserialize object of type '{type.Name}'", inner);
    }
}
