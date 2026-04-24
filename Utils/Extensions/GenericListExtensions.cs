namespace Utils.Extensions;

/// <summary>
/// This set of extensions are meant to be useful List<T> methods 
/// that are not defined for IEnumerable<T>, even though they could be.
/// They all use the default implementations of List<T> if the argument is indeed a List,
/// but a generic implementation otherwise, which may not be so performatic.
/// </summary>
public static class GenericListExtensions
{
    /// <summary>
    /// Adds a range of elements to the end of the enumerable.
    /// If the enumerable is a <see cref="List{T}" />, uses its AddRange method for efficiency.
    /// Otherwise, concatenates the elements, setting the enumerable to the new concatenated enumerable.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="enumerable">The target enumerable to add elements to.</param>
    /// <param name="values">The elements to add.</param>
    public static void AddRange<T>(this IEnumerable<T> enumerable, IEnumerable<T> values)
    {
        if (enumerable is List<T> list)
        {
            list.AddRange(values);
        }
        enumerable = enumerable.Concat(values);
    }

    /// <summary>
    /// Converts all elements in the enumerable to another type using the specified mapping function.
    /// If the enumerable is a <see cref="List{T}" />, uses its ConvertAll method for efficiency.
    /// Otherwise, applies the mapping function to each element using LINQ's Select.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source enumerable.</typeparam>
    /// <typeparam name="U">The type of elements in the resulting enumerable.</typeparam>
    /// <param name="enumerable">The source enumerable to convert.</param>
    /// <param name="mapper">A function to convert each element.</param>
    /// <returns>An <see cref="IEnumerable{U}" />; containing the converted elements.</returns>
    public static IEnumerable<U> ConvertAll<T, U>(this IEnumerable<T> enumerable, Func<T, U> mapper)
    {
        if (enumerable is List<T> list)
        {
            return list.ConvertAll(new Converter<T, U>(mapper));
        }
        return enumerable.Select(mapper);
    }

    /// <summary>
    /// Performs the specified action on each element of the enumerable.
    /// If the enumerable is a <see cref="List{T}" />, uses its ForEach method for efficiency.
    /// Otherwise, iterates through the enumerable and applies the action to each element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="enumerable">The enumerable whose elements the action will be performed on.</param>
    /// <param name="action">The action to perform on each element.</param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        if (enumerable is List<T> list)
        {
            list.ForEach(action);
        }
        foreach (T value in enumerable)
        {
            action(value);
        }
    }
}