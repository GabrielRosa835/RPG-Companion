namespace RpgCompanion.Toolbox.Storage;

public class ConcurrentSimpleStorage : IStorage
{
    public bool Contains<T>(StorageKey<T> key)
    {
        throw new NotImplementedException();
    }

    public bool Add<T>(StorageKey<T> key, T value)
    {
        throw new NotImplementedException();
    }

    public bool Remove<T>(StorageKey<T> key)
    {
        throw new NotImplementedException();
    }

    public T? Put<T>(StorageKey<T> key, T value)
    {
        throw new NotImplementedException();
    }

    public T Get<T>(StorageKey<T> key)
    {
        throw new NotImplementedException();
    }

    public T? GetOrDefault<T>(StorageKey<T> key)
    {
        throw new NotImplementedException();
    }

    public T Acquire<T>(StorageKey<T> key)
    {
        throw new NotImplementedException();
    }

    public T? AcquireOrDefault<T>(StorageKey<T> key)
    {
        throw new NotImplementedException();
    }
}
