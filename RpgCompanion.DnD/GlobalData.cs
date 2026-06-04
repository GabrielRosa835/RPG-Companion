namespace RpgCompanion.DnD;

using Core;
using Utils.Storage;

public class GlobalData : DynamicStorage, IActor
{
    public static ActorKey<GlobalData> Key { get; } = typeof(GlobalData).FullName!;
}
