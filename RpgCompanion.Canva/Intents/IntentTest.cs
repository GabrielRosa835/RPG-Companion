namespace RpgCompanion.Canva.Intents;

using Core;
using Microsoft.Extensions.Logging;

public static class IntentTest
{
    public static async Task Run(IIntentDispatcher dispatcher, ILogger<Initialization> logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started intent processing");

        var task = dispatcher.Dispatch(new Intent
        {
            NumberValue = 10,
            TextValue = "Teste",
        }, cancellationToken);

        var resultTask = dispatcher.Dispatch(new Intent2
        {
            NumberValue = 20,
            TextValue = "Teste2",
        }, cancellationToken);

        await Task.WhenAll(task, resultTask);

        logger.LogInformation("Intent 2 processed with result: {0}", resultTask.Result);
        logger.LogInformation("Finished intent processing");
    }
}
