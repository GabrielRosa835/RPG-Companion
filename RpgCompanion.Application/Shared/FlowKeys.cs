using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Application;

public static class FlowKeys
{
    public static readonly ContextKey<bool> IsAfterEffect = new(nameof(IsAfterEffect));
    public static readonly ContextKey<bool> HasEffectApplied = new(nameof(IsAfterEffect));
}