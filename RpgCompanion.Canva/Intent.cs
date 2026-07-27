namespace RpgCompanion.Canva;

using Core;

public class Intent : IIntent, IIntentProcessor<Intent>
{
    public string TextValue { get; init; } = default!;
    public int NumberValue { get; init; }

    public void Process(Intent intent, IIntentContext context)
    {
        Console.WriteLine($"{intent.TextValue} - {intent.NumberValue}");
    }
}
