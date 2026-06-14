namespace RpgCompanion.Host;

using Core;

internal abstract class SerializerHelper
{
    public abstract ISerializationContext Serialize(
        object value,
        ISerializationContext context,
        IServiceProvider serviceProvider);

    public static SerializerHelper? TryCreateTyped(Type type, IServiceProvider serviceProvider)
    {
        var serializerType = typeof(ISerializer<>).MakeGenericType(type);
        var serializer = serviceProvider.GetService(serializerType);
        if (serializer is null) return null;
        var helperType = typeof(TypedSerializerHelper<>).MakeGenericType(type);
        return Activator.CreateInstance(helperType) as SerializerHelper;
    }

    private class TypedSerializerHelper<T> : SerializerHelper
    {
        public override ISerializationContext Serialize(
            object value,
            ISerializationContext context,
            IServiceProvider serviceProvider)
        {
            var serializer = serviceProvider.GetRequiredService<ISerializer<T>>();
            serializer.Serialize((T) value, context);
            return context;
        }
    }
}
