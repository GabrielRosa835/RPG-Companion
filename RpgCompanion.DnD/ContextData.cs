namespace RpgCompanion.DnD;

using Core;
using Utils.Storage;

public class ContextData : DynamicStorage, IActor, IDisposable
{
    public static ActorKey<ContextData> Key { get; } = typeof(ContextData).FullName!;

    public void Dispose()
    {
        Console.WriteLine($"{nameof(ContextData)} disposed");
    }
}
