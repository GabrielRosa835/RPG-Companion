namespace RpgCompanion.DnD._Old;

using RpgCompanion.Toolbox;

public class GlobalData : DynamicStorage, IActor
{
    public static ActorKey<GlobalData> Key { get; } = typeof(GlobalData).FullName!;
}
