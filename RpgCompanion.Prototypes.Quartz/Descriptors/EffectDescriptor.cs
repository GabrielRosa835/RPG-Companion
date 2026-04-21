namespace RpgCompanion.QuartzPrototype.Descriptors;

internal class EffectDescriptor(string key) : IEquatable<EffectDescriptor>
{
    public EffectKey Key { get; } = new(key);
    public string? DisplayName { get; set; }

    public bool Equals(EffectDescriptor? other) => other is not null && this.Key.Equals(other.Key);
    public override bool Equals(object? obj) => obj is EffectDescriptor other && Equals(other);
    public override int GetHashCode() => this.Key.GetHashCode();
}

internal record struct EffectKey(string Value);
