namespace RpgCompanion.DnD;

using Core;

public class GlobalData : DynamicStorage, IActor
{
    public static ActorKey<GlobalData> Key { get; } = typeof(GlobalData).FullName!;
}
