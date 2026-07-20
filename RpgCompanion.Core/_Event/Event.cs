namespace RpgCompanion.Core;

public abstract record Event(EventSetup EventSetup, EventExecutor EventExecutor, EventTeardown EventTeardown) : EventSetup;

public interface IEventTemplate
{
    static abstract EventSetup Setup { get; }
    static abstract EventExecutor Execute { get; }
    static abstract EventTeardown Teardown { get; }
}
