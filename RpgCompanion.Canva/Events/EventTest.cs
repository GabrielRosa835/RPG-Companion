namespace RpgCompanion.Canva.Events;

using Core;
using Microsoft.Extensions.Logging;

public static class EventTest
{
    public static async Task Run(IEventTrigger trigger, ILogger<Initialization> logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started event processing");

        var task3 = trigger.Raise(new Event3(), cancellationToken);
        var task2 = trigger.Raise(new Event2(), cancellationToken);
        var task1 = trigger.Raise(new Event1(), cancellationToken);
        await Task.WhenAll(task1, task2, task3);

        logger.LogInformation("Event result 1: {0}", task1.Result.GetType().Name);
        logger.LogInformation("Event result 2: {0}", task2.Result.GetType().Name);
        logger.LogInformation("Event result 3: {0}", task3.Result.GetType().Name);
        logger.LogInformation("Finished event processing");
    }
}
