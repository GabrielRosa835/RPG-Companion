namespace RpgCompanion.Toolbox;

/// <summary>
/// Represents a type with a single value. This type is used to signify
/// the absence of a meaningful value in generic types (acting as a 'void' substitute).
/// </summary>
public readonly record struct None : IComparable<None>
{
    public static None Value { get; } = new();
    public override string ToString () => "()";
    public int CompareTo (None other) => 0;
}
