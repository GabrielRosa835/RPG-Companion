namespace RpgCompanion.QuartzPrototype.Descriptors;

internal class EventDescriptor(string key) : IEquatable<EventDescriptor>
{
    public EventKey Key { get; } = new(key);
    public EffectKey Effect { get; set; }
    public List<RuleKey> Rules { get; set; } = [];
    public string? DisplayName { get; set; }
    public int Priority { get; set; }

    public bool Equals(EventDescriptor? other) => other is not null && this.Key.Equals(other.Key);
    public override bool Equals(object? obj) => obj is EventDescriptor other && Equals(other);
    public override int GetHashCode() => this.Key.GetHashCode();
}

internal record struct EventKey(string Value);
