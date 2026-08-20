namespace RpgCompanion.Host.Storage;

internal class StorageCreationOptions : IStorageCreationOptions
{
    internal bool InMemoryValue { get; private set; } = false;
    internal bool DynamicValue { get; private set; } = false;
    internal bool ConcurrentValue { get; private set; } = false;

    public IStorageCreationOptions InMemory(bool set = true)
    {
        InMemoryValue = set;
        return this;
    }

    public IStorageCreationOptions Dynamic(bool set = true)
    {
        DynamicValue = set;
        return this;
    }

    public IStorageCreationOptions Concurrent(bool set = true)
    {
        ConcurrentValue = set;
        return this;
    }
}
