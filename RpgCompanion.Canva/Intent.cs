namespace RpgCompanion.Canva;

using Core;

public class Intent : IIntent, IIntentHandlerTemplate<Intent>
{
    public string TextValue { get; init; } = default!;
    public int NumberValue { get; init; }

    public static void Handle(Intent intent, IIntentContext context)
    {
        Console.WriteLine($"{intent.TextValue} - {intent.NumberValue}");
    }
}
