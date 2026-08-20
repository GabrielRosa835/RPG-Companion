namespace RpgCompanion.Core;

public interface IStorageCreationOptions
{
    public IStorageCreationOptions InMemory(bool set = true);
    public IStorageCreationOptions Dynamic(bool set = true);
    public IStorageCreationOptions Concurrent(bool set = true);
}

public interface IStorageFactory
{
    public IStorage Create(Action<IStorageCreationOptions> configure);
}
