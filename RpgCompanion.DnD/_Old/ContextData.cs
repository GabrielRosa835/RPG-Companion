namespace RpgCompanion.DnD._Old;

using RpgCompanion.Toolbox;

public class ContextData : DynamicStorage, IActor, IDisposable
{
    public static ActorKey<ContextData> Key { get; } = typeof(ContextData).FullName!;

    public void Dispose()
    {
        Console.WriteLine($"{nameof(ContextData)} disposed");
    }
}
