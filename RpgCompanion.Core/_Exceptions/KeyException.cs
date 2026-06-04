namespace RpgCompanion.Core;

using System.Diagnostics.CodeAnalysis;

public class KeyException : ConfigurationException
{
    public KeyException(string message) : base(message)
    {
    }

    public KeyException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull<T>([NotNull] T? key) where T : struct
    {
        if (key is null) throw new KeyException($"Missing '{nameof(T)}'");
    }

}
