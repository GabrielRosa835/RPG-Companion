namespace RpgCompanion.Host;

using Core;

internal abstract class DeserializerHelper
{
    public abstract object? Deserialize(IDeserializationContext context, IServiceProvider serviceProvider);

    public static DeserializerHelper? TryCreateTyped(Type type, IServiceProvider serviceProvider)
    {
        var serializerType = typeof(ISerializer<>).MakeGenericType(type);
        var serializer = serviceProvider.GetService(serializerType);
        if (serializer is null) return null;
        var helperType = typeof(TypedDeserializerHelper<>).MakeGenericType(type);
        return Activator.CreateInstance(helperType) as DeserializerHelper;
    }

    private class TypedDeserializerHelper<T> : DeserializerHelper
    {
        public override object? Deserialize(IDeserializationContext context, IServiceProvider serviceProvider)
        {
            var serializer = serviceProvider.GetRequiredService<ISerializer<T>>();
            return serializer.Deserialize(context);
        }
    }
}
