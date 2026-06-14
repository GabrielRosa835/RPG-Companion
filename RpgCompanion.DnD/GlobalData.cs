namespace RpgCompanion.DnD;

using Core;
using Core.Toolbox;

public class GlobalData : DynamicStorage, IActor
{
    public static ActorKey<GlobalData> Key { get; } = typeof(GlobalData).FullName!;
}
