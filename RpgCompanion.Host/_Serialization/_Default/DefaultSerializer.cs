namespace RpgCompanion.Host;

using System.Collections;
using System.Collections.Concurrent;
using Core;
using Core.Toolbox;

internal class DefaultSerializer(IServiceProvider _serviceProvider)
{
    private static readonly ConcurrentDictionary<Type, PropertyMetadata[]> MetadataCache = new();
    private static readonly ConcurrentDictionary<Type, CollectionDeserializerHelper> CollectionHelperCache = new();
    private static readonly ConcurrentDictionary<Type, DeserializerHelper?> DeserializerCache = new();
    private static readonly ConcurrentDictionary<Type, SerializerHelper?> SerializerCache = new();

    public ISerializationContext Serialize(object? value, Type type, ISerializationContext context)
    {
        if (value is null) return context.Null();

        var serializer = SerializerCache.GetOrAdd(type, t => SerializerHelper.TryCreateTyped(t, _serviceProvider));
        if (serializer is not null) return serializer.Serialize(value, context, _serviceProvider);

        if (value is string str) return context.String(str);
        if (value is bool b) return context.Boolean(b);
        if (value is byte by) return context.Number(by);
        if (value is sbyte sby) return context.Number(sby);
        if (value is short s) return context.Number(s);
        if (value is ushort us) return context.Number(us);
        if (value is int i) return context.Number(i);
        if (value is uint ui) return context.Number(ui);
        if (value is long l) return context.Number(l);
        if (value is ulong ul) return context.Number(ul);
        if (value is float f) return context.Number(f);
        if (value is double dbl) return context.Number(dbl);
        if (value is decimal dec) return context.Number(dec);

        if (value.GetType().IsEnum) return context.String(value.ToString() ?? string.Empty);

        if (value is IEnumerable enumerable)
        {
            return context.Array(nestedCtx =>
            {
                foreach (var item in enumerable)
                {
                    if (item is null)
                    {
                        nestedCtx.Null();
                        continue;
                    }
                    Serialize(item, item.GetType(), nestedCtx);
                }
            });
        }

        if (type.IsComplex)
        {
            var properties = MetadataCache.GetOrAdd(type, PropertyMetadata.GetProperties);
            foreach (var propMeta in properties)
            {
                if (!propMeta.CanRead) continue;
                var propValue = propMeta.Info.GetValue(value);
                context.Field(propMeta.JsonName, fieldCtx => Serialize(propValue, propMeta.Type, fieldCtx));
            }
        }

        return context;
    }

    public object? Deserialize(Type type, IDeserializationContext context)
    {
        if (context.IsNull()) return null;

        var deserializer =
            DeserializerCache.GetOrAdd(type, t => DeserializerHelper.TryCreateTyped(t, _serviceProvider));
        if (deserializer is not null) return deserializer.Deserialize(context, _serviceProvider);

        if (type == typeof(string)) return context.GetString();
        if (type == typeof(bool)) return context.GetBoolean();
        if (type == typeof(byte)) return context.GetNumber<byte>();
        if (type == typeof(sbyte)) return context.GetNumber<sbyte>();
        if (type == typeof(short)) return context.GetNumber<short>();
        if (type == typeof(ushort)) return context.GetNumber<ushort>();
        if (type == typeof(int)) return context.GetNumber<int>();
        if (type == typeof(uint)) return context.GetNumber<uint>();
        if (type == typeof(long)) return context.GetNumber<long>();
        if (type == typeof(ulong)) return context.GetNumber<ulong>();
        if (type == typeof(float)) return context.GetNumber<float>();
        if (type == typeof(double)) return context.GetNumber<double>();
        if (type == typeof(decimal)) return context.GetNumber<decimal>();

        if (type.IsEnum)
        {
            var enumString = context.GetString();
            return string.IsNullOrEmpty(enumString) ? null : Enum.Parse(type, enumString, ignoreCase: true);
        }

        if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type))
        {
            var elementType = type.IsArray
                ? type.GetElementType()!
                : type.GetGenericArguments().FirstOrDefault() ?? typeof(object);

            var helper = CollectionHelperCache.GetOrAdd(elementType, CollectionDeserializerHelper.CreateTyped);
            return helper.Deserialize(type, context, this);
        }

        if (type.IsComplex)
        {
            // Value types don't require parameterless constructors
            if (!type.IsValueType && type.GetConstructor(Type.EmptyTypes) is null)
            {
                throw SerializationExceptions.ParameterlessConstructorRequiredException(type);
            }

            object instance = Activator.CreateInstance(type)!;
            var properties = MetadataCache.GetOrAdd(type, PropertyMetadata.GetProperties);

            foreach (var propMeta in properties)
            {
                if (!propMeta.CanWrite) continue;
                var found = context.TryGetField<object?>(
                    propMeta.JsonName,
                    fieldCtx => Deserialize(propMeta.Type, fieldCtx),
                    out var propValue);
                if (found)
                {
                    propMeta.Info.SetValue(instance, propValue);
                }
            }
            return instance;
        }

        return null;
    }
}
