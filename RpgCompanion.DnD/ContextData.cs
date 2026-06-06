namespace RpgCompanion.DnD;

using Core;

public class ContextData : DynamicStorage, IActor, IDisposable
{
    public static ActorKey<ContextData> Key { get; } = typeof(ContextData).FullName!;

    public void Dispose()
    {
        Console.WriteLine($"{nameof(ContextData)} disposed");
    }
}
