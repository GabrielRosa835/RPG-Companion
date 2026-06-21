namespace RpgCompanion.Core;

public abstract class RuleContext : IRegistry
{
    public object? Subject { get; set; }
    public World World { get; init; } = default!;
    public abstract TActor? GetOrDefault<TActor>() where TActor : class, IActor;
    public abstract TActor Get<TActor>() where TActor : class, IActor;
}
