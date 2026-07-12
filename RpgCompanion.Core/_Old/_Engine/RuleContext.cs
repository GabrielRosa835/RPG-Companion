namespace RpgCompanion.Events;

using Core;
using Toolbox;

public abstract class RuleContext : IRegistry, IStorage, ITrigger
{
    public abstract World World { get; }

    // Registry
    public abstract TActor? GetService<TActor>() where TActor : class, IActor;
    public abstract TActor GetService<TActor>() where TActor : class, IActor;

    // Storage
    public abstract void Add<T>(StorageKey<T> storageKey, T value);
    public abstract void Put<T>(StorageKey<T> storageKey, T value);
    public abstract void Remove<T>(StorageKey<T> storageKey);
    public abstract T Get<T>(StorageKey<T> storageKey);
    public abstract T? GetOrDefault<T>(StorageKey<T> storageKey);
    public abstract T Acquire<T>(StorageKey<T> storageKey);
    public abstract T? AcquireOrDefault<T>(StorageKey<T> storageKey);

    // Trigger
    public abstract void Raise<TEvent>(TEvent e, Action<IPipeline<TEvent>>? pipeline = null) where TEvent : IEvent;
}
