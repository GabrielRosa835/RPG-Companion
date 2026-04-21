namespace RpgCompanion.Host.Descriptors;

using Core.Events;

internal class RuleDescriptor : IEquatable<RuleDescriptor>
{
    public RuleKey Key { get; init; }
    public EventKey Event { get; init; }
    public RuleOrdering Ordering { get; init; }
    public string? DisplayName { get; init; }

    public bool Equals(RuleDescriptor? other) => other is not null && this.Key.Equals(other.Key);
    public override bool Equals(object? obj) => obj is RuleDescriptor other && Equals(other);
    public override int GetHashCode() => this.Key.GetHashCode();
}

public record struct RuleKey(string Value)
{
    public RuleKey() : this(Guid.NewGuid().ToString())
    {
    }
}
