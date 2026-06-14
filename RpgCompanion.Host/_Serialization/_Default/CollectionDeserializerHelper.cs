namespace RpgCompanion.Host;

using Core;

internal abstract class CollectionDeserializerHelper
{
    public abstract object? Deserialize(Type targetType, IDeserializationContext context, DefaultSerializer serializer);

    public static CollectionDeserializerHelper CreateTyped(Type elementType)
    {
        var helperType = typeof(TypedCollectionDeserializerHelper<>).MakeGenericType(elementType);
        return (CollectionDeserializerHelper) Activator.CreateInstance(helperType)!;
    }

    private class TypedCollectionDeserializerHelper<TElement> : CollectionDeserializerHelper
    {
        public override object? Deserialize(Type targetType, IDeserializationContext context,
            DefaultSerializer serializer)
        {
            var items = context.GetArray(ctx => (TElement) serializer.Deserialize(typeof(TElement), ctx)!);
            if (targetType.IsArray) return items.ToArray();
            return items.ToList();
        }
    }
}
