namespace RpgCompanion.Canva;

using Core;
using Microsoft.Extensions.Logging;

public class Intent : IIntent, IIntentHandler<Intent>
{
    public string TextValue { get; init; } = default!;
    public int NumberValue { get; init; }

    public void Handle(Intent intent, IntentContext context)
    {
        var logger = context.Host.Registry.Get<ILogger<Intent>>();
        for (int i = 0; i < 5; i++)
        {
            logger.LogInformation("{0}. {1} - {2}", i, intent.NumberValue, intent.TextValue);
        }
    }
}
