namespace RpgCompanion.DnD;

public readonly record struct AbilityScoreValue
{
    private readonly byte _value;

    private AbilityScoreValue(byte value)
    {
        if (value is < 1 or > 30)
            throw new ArgumentOutOfRangeException(nameof(value), "Ability score must be between 1 and 30.");

        _value = value;
    }
    public static implicit operator AbilityScoreValue(int value) => new((byte)value);
    public static implicit operator int(AbilityScoreValue score) => score._value;
}
