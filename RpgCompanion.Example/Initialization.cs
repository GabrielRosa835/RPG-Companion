namespace RpgCompanion.Canva;

using Core;
using Events;
using Intents;
using Microsoft.Extensions.Logging;
// using Persistence;

public class Initialization(
    IEventTrigger _trigger,
    IIntentDispatcher _dispatcher,
    IDatabase _database)
    : IAsyncInitialization
{
    public async Task Initialize(IInitializationContext context, CancellationToken cancellationToken)
    {
        var logger = context.Host.Registry.Get<ILogger<Initialization>>();

        logger.LogInformation("Initializing RpgCompanion.Canvas");

        await EventTest.Run(_trigger, logger, cancellationToken);
        await IntentTest.Run(_dispatcher, logger, cancellationToken);
        // await PersistenceTest.Run(_database, logger, cancellationToken);

        logger.LogInformation("Finished Initializing RpgCompanion.Canvas");
    }
}
