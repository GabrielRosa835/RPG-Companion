namespace RpgCompanion.Toolbox;

public interface IStorage
{
    /// <summary>
    /// Adds the element if none already exists
    /// </summary>
    void Add<T>(StorageKey<T> storageKey, T value);

    /// <summary>
    /// Adds the element or updates it if one already exists
    /// </summary>
    void Put<T>(StorageKey<T> storageKey, T value);

    /// <summary>
    /// Removes the referenced element if one exists
    /// </summary>
    void Remove<T>(StorageKey<T> storageKey);

    /// <summary>
    /// Retrieves the referenced element, throwing when none exists
    /// </summary>
    T Get<T>(StorageKey<T> storageKey);

    /// <summary>
    /// Tries to retrieve the referenced element or a default value if none exists
    /// </summary>
    T? GetOrDefault<T>(StorageKey<T> storageKey);

    /// <summary>
    /// Retrieves the referenced element, throwing when none exists, removing it afterward
    /// </summary>
    T Acquire<T>(StorageKey<T> storageKey);

    /// <summary>
    /// Tries to retrieve the referenced element or a default value if none exists, removing it afterward
    /// </summary>
    T? AcquireOrDefault<T>(StorageKey<T> storageKey);
}
