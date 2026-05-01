namespace RpgCompanion.Core;

public readonly record struct RuleKey(string Content)
{
    public static implicit operator RuleKey(string content) => new(content);
    public RuleKey<T> As<T>() => new(Content);
    public RuleKey<T, U> As<T, U>() => new(Content);
}

public readonly record struct RuleKey<T>(string Content)
{
    public static implicit operator RuleKey<T>(string content) => new(content);
    public static implicit operator RuleKey(RuleKey<T> key) => new(key.Content);
}

public readonly record struct RuleKey<T, U>(string Content)
{
    public static implicit operator RuleKey<T, U>(string content) => new(content);
    public static implicit operator RuleKey(RuleKey<T, U> key) => new(key.Content);
}
