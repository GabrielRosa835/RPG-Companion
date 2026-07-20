namespace RpgCompanion.Canva;

using Core;

public class Manifest : IManifest
{
    public void Configure(IPluginConfiguration plugin)
    {
        plugin.WithName("Canva");
        plugin.WithVersion("1.0.0");
        plugin.AddIntent<Intent>(Intent.Handle);
        plugin.WithInitialization(Initialization.Handle);
    }
}

public class Initialization : IInitializationHandlerAsyncTemplate
{
    public static async Task Handle(IInitializationContextAsync context)
    {
        var trigger = context.Registry.Get<IEventTrigger>();
    }
}
