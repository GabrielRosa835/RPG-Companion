namespace RpgCompanion.Host.Descriptors;

internal class EventDescriptor : IEquatable<EventDescriptor>
{
    public EventKey Key { get; init; }
    public EffectKey Effect { get; init; }
    public IReadOnlySet<RuleKey> Rules { get; init; }
    public string? DisplayName { get; init; }
    public int Priority { get; init; }

    public bool Equals(EventDescriptor? other) => other is not null && this.Key.Equals(other.Key);
    public override bool Equals(object? obj) => obj is EventDescriptor other && Equals(other);
    public override int GetHashCode() => this.Key.GetHashCode();
}

internal record struct EventKey(string Value);
