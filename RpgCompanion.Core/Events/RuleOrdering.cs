namespace RpgCompanion.Application.Services;

public enum RuleOrdering
{
    Before = 1 << 0,
    After = 1 << 1,
    Both = Before | After,
}