namespace RpgCompanion.Core;

using System.Collections.Concurrent;

public static class Model
{
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, ModelContent.Object>> _extensions = new();

    public static ModelContent.Object Extensions(object subject)
    {
        ArgumentNullException.ThrowIfNull(subject);
        if (!_extensions.TryGetValue(subject.GetType(), out var extensions))
        {
            extensions = new();
            _extensions[subject.GetType()] = extensions;
        }
        if (!extensions.TryGetValue(subject, out var extension))
        {
            extension = new();
            extensions[subject] = extension;
        }
        return extension;
    }

    public static ModelContent Get(object subject, string fieldName)
    {
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(fieldName);
        var extensions = Extensions(subject);
        if (!extensions.Properties.TryGetValue(fieldName, out var content))
        {
            return new ModelContent.None();
        }
        return content;
    }

    public static void Set(object subject, string fieldName, ModelContent content)
    {
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(fieldName);
        var extensions = Extensions(subject);
        extensions.Properties[fieldName] = content;
    }
}
