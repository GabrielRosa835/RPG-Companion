namespace RpgCompanion.Canva.Initialization;

using Core.Engine;
using Core.Meta;
using Pipeline;

public class CanvaManifest : IManifest
{
    public void Configure(IPluginBuilder builder) => builder
        .WithMetadata(plugin => plugin
            .WithName("Canva")
            .WithVersion("1.0.0"))
        .WithInitialization(initialization => initialization
            .WithAction(Initialize))
        .AddEvent<ConsoleRead>(e => e
            .WithName(nameof(ConsoleRead))
            .AddEffect(effect => effect
                .WithAction(ReadConsole)))
        .AddEvent<ConsoleWriteLine>(e => e
            .WithName(nameof(ConsoleWriteLine))
            .AddEffect(effect => effect
                .WithAction(PrintConsoleLine)))
        .AddEvent<ConsoleWrite>(e => e
            .WithName(nameof(ConsoleWrite))
            .AddEffect(effect => effect
                .WithAction(PrintConsole)));

    private static void Initialize(IRegistry registry)
    {
        Console.WriteLine("Canva Initialized");
        var pipeline = registry.GetRequired<IPipeline>();
        pipeline.Raise(new ConsoleWriteLine("Initialized"))
            .FollowedBy(w => new ConsoleWrite("Digite algo: "))
            .FollowedBy(w => new ConsoleRead())
            .FollowedBy(r => new ConsoleWriteLine(r.Input));
    }

    private static void PrintConsoleLine(ConsoleWriteLine e, IPipeline pipeline)
    {
        Console.WriteLine(e.Message);
    }
    private static void PrintConsole(ConsoleWrite e, IPipeline pipeline)
    {
        Console.Write(e.Message);
    }

    private static void ReadConsole(ConsoleRead e, IPipeline pipeline)
    {
        var value = "";
        while (string.IsNullOrWhiteSpace(value))
        {
            value = Console.ReadLine();
        }
        e.Input = value;
    }
}
