namespace RpgCompanion.Canva;

using Core;

public record Event1() : Event(Setup, Execute, Teardown), IEventTemplate
{
    public static EventSetup Setup => new EventSetup.Sync((IEventContext ctx) =>
    {
        Console.WriteLine($"{nameof(Event1)}.{nameof(Setup)} called");
        return new EventResult.None();
    });

    public static EventExecutor Execute => new EventExecutor.Sync((IEventContext ctx) =>
    {
        Console.WriteLine($"{nameof(Event1)}.{nameof(Execute)} called");
        return new EventResult.Continue(new Event2());
    });

    public static EventTeardown Teardown => new EventTeardown.Sync((IEventContext ctx) =>
    {
        Console.WriteLine($"{nameof(Event1)}.{nameof(Teardown)} called");
        return new EventResult.None();
    });
}
