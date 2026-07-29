namespace RpgCompanion.Canva;

using Core;
using Microsoft.Extensions.Logging;

public class Intent2 : IIntent<string>, IIntentProcessor<Intent2, string>
{
    public string TextValue { get; init; } = default!;
    public int NumberValue { get; init; }

    public string Process(Intent2 intent, IIntentContext context)
    {
        var logger = context.Host.Registry.Get<ILogger<Intent2>>();
        for (int i = 0; i < 5; i++)
        {
            logger.LogInformation("{0}. {1} - {2}", i, intent.NumberValue, intent.TextValue);
        }
        return intent.TextValue;
    }
}
