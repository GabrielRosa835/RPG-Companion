namespace RpgCompanion.Host;

using System.Reflection;

internal record PropertyMetadata
{
    public required PropertyInfo Info { get; init; }
    public required string JsonName { get; init; }
    public required Type Type { get; init; }
    public required bool CanRead { get; init; }
    public required bool CanWrite { get; init; }

    public static PropertyMetadata[] GetProperties(Type type)
    {
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var metaList = new PropertyMetadata [props.Length];
        for (int i = 0; i < props.Length; i++)
        {
            var prop = props[i];
            metaList[i] = new PropertyMetadata
            {
                Info = prop,
                JsonName = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1),
                Type = prop.PropertyType,
                CanRead = prop.CanRead,
                CanWrite = prop.CanWrite
            };
        }
        return metaList;
    }
}
