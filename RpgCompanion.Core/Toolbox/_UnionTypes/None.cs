namespace RpgCompanion.Toolbox;

/// <summary>
/// Represents a type with a single value. This type is used to signify
/// the absence of a meaningful value in generic types (acting as a 'void' substitute).
/// </summary>
public readonly struct None : IEquatable<None>, IComparable<None>
{
    private static readonly None _value = new();

    /// <summary>
    /// Gets the single instance of the <see cref="None"/> value.
    /// </summary>
    public static None Value => _value;

    /// <summary>
    /// Returns a string representation of the unit value.
    /// </summary>
    public override string ToString () => "()";

    public bool Equals (None other) => true;
    public override bool Equals (object? obj) => obj is None;
    public override int GetHashCode () => 0;
    public int CompareTo (None other) => 0;
    public static bool operator == (None first, None second) => true;
    public static bool operator != (None first, None second) => false;
}

/// <summary>
/// Exceção lançada quando uma operação espera um valor, mas apenas <see cref="None"/> está presente.
/// Usada para sinalizar ausência de valor significativo em contextos onde <see cref="None"/> é utilizado como substituto de 'void'.
/// </summary>
public class NoneException : Exception
{
    public NoneException () : base("No value present") { }
}
