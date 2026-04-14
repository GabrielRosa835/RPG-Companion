namespace RpgCompanion.Core.Events;

[Flags]
public enum RuleOrdering
{
    None = 0,
    Before = 1,
    After = 2,
    BeforeAndAfter = Before | After,
}
