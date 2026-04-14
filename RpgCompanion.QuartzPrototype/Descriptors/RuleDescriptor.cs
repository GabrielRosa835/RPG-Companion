namespace RpgCompanion.QuartzPrototype.Descriptors;

using Core.Events;

internal class RuleDescriptor(string key) : IEquatable<RuleDescriptor>
{
    public RuleKey Key { get; } = new(key);
    public string? DisplayName { get; set; }
    public RuleOrdering Ordering { get; set; } = RuleOrdering.None;

    public bool Equals(RuleDescriptor? other) => other is not null && this.Key.Equals(other.Key);
    public override bool Equals(object? obj) => obj is RuleDescriptor other && Equals(other);
    public override int GetHashCode() => this.Key.GetHashCode();
}

public record struct RuleKey(string Value);
