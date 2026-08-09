namespace RpgCompanion.Toolbox;

public interface IStorage
{
    /// <summary>
    /// Checks if the element does exist
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>true if the element does exist</returns>
    bool Contains<T>(StorageKey<T> key);

    /// <summary>
    /// Adds the element if none already exists
    /// </summary>
    /// <returns>true if the element was added</returns>
    bool Add<T>(StorageKey<T> key, T value);

    /// <summary>
    /// Removes the referenced element if one exists
    /// </summary>
    /// <returns>true if the element was removed</returns>
    bool Remove<T>(StorageKey<T> key);

    /// <summary>
    /// Adds the element or updates it if one already exists
    /// </summary>
    /// <returns>The previous element if one existed</returns>
    T? Put<T>(StorageKey<T> key, T value);

    /// <summary>
    /// Retrieves the referenced element, throwing when none exists
    /// </summary>
    T Get<T>(StorageKey<T> key);

    /// <summary>
    /// Tries to retrieve the referenced element or a default value if none exists
    /// </summary>
    T? GetOrDefault<T>(StorageKey<T> key);

    /// <summary>
    /// Retrieves the referenced element, throwing when none exists, removing it afterward
    /// </summary>
    T Acquire<T>(StorageKey<T> key);

    /// <summary>
    /// Tries to retrieve the referenced element or a default value if none exists, removing it afterward
    /// </summary>
    T? AcquireOrDefault<T>(StorageKey<T> key);
}
