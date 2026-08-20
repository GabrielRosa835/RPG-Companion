namespace RpgCompanion.Canva;

using Core;
using Events;
using Microsoft.Extensions.Logging;

public class Manifest : IManifest
{
    public void Configure(IPluginConfiguration plugin, IRegistry hostRegistry)
    {
        var logger = hostRegistry.Get<ILogger<Manifest>>();

        logger.LogInformation("Configuring RpgCompanion.Canvas");
        plugin.WithName("Canva");
        plugin.WithVersion("1.0.0");
        plugin.WithIdentifier("canva");

        plugin.AddEvent<Event1>();
        plugin.AddEvent<Event2>();
        plugin.AddEvent<Event3>();

        plugin.AddIntent<Intent>(i => i.WithProcessor<Intent>());
        plugin.AddIntent<Intent2, string>(i => i.WithProcessor<Intent2>());

        plugin.AddEntity<Entity>();
        plugin.AddEntity<OtherEntity>();

        plugin.WithAsyncInitialization<Initialization>();

        logger.LogInformation("Finished Configuring RpgCompanion.Canvas");
    }
}
