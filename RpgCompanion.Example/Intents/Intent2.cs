namespace RpgCompanion.Canva;

using Core;
using Microsoft.Extensions.Logging;

public class Intent2 : IIntent<string>, IIntentHandler<Intent2, string>
{
    public string TextValue { get; init; } = default!;
    public int NumberValue { get; init; }

    public string Handle(Intent2 intent, IntentContext context)
    {
        var logger = context.Host.Registry.Get<ILogger<Intent2>>();
        for (int i = 0; i < 5; i++)
        {
            logger.LogInformation("{0}. {1} - {2}", i, intent.NumberValue, intent.TextValue);
        }
        return intent.TextValue;
    }
}
