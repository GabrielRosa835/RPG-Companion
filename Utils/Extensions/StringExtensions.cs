namespace Utils.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// TrimStart overload to accept string value instead of char array,
    /// since the original TrimStart only accepts char array.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string TrimStart(this string source, string value)
    {
        char[] valueAsArray = value.ToArray();
        return source.TrimStart(valueAsArray);
    }
}