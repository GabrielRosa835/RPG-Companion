namespace RpgCompanion.Host.Storage;

using Providers;

public class StorageFactory : IStorageFactory
{
    public IStorage Create(Action<IStorageCreationOptions> configure)
    {
        var options = new StorageCreationOptions();
        configure(options);
        return (options.ConcurrentValue, options.DynamicValue, options.InMemoryValue) switch
        {
            (true, true, true) => new ConcurrentDynamicStorage(),
            (false, true, true) => new DynamicStorage(),
            (true, false, true) => new ConcurrentSimpleStorage(),
            (false, false, true) => new SimpleStorage(),
            _ => null!,
        };
    }
}
