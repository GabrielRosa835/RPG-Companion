namespace RpgCompanion.Host.Descriptors;

internal class EffectDescriptor : IEquatable<EffectDescriptor>
{
    public EffectKey Key { get; init; }
    public EventKey Event { get; init; }
    public string? DisplayName { get; init; }

    public bool Equals(EffectDescriptor? other) => other is not null && this.Key.Equals(other.Key);
    public override bool Equals(object? obj) => obj is EffectDescriptor other && Equals(other);
    public override int GetHashCode() => this.Key.GetHashCode();
}

internal record struct EffectKey(string Value)
{
    public EffectKey() : this(Guid.NewGuid().ToString())
    {
    }
}
