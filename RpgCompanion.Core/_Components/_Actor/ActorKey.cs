namespace RpgCompanion.Core;

public readonly record struct ActorKey(string content)
{
    public static implicit operator ActorKey(string content) => new(content);
}

public readonly record struct ActorKey<TActor>(string Content) where TActor : class, IActor
{
    public static implicit operator ActorKey<TActor>(string content) => new(content);
    public static implicit operator ActorKey(ActorKey<TActor> key) => new(key.Content);
}
