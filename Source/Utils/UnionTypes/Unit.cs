namespace Utils.UnionTypes;

/// <summary>
/// Represents a type with a single value. This type is used to signify 
/// the absence of a meaningful value in generic types (acting as a 'void' substitute).
/// </summary>
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>
{
    private static readonly Unit _value = new();

    /// <summary>
    /// Gets the single instance of the <see cref="Unit"/> value.
    /// </summary>
    public static Unit Value => _value;

    /// <summary>
    /// Returns a string representation of the unit value.
    /// </summary>
    public override string ToString () => "()";

    public bool Equals (Unit other) => true;
    public override bool Equals (object obj) => obj is Unit;
    public override int GetHashCode () => 0;
    public int CompareTo (Unit other) => 0;
    public static bool operator == (Unit first, Unit second) => true;
    public static bool operator != (Unit first, Unit second) => false;
}

/// <summary>
/// Exceção lançada quando uma operação espera um valor, mas apenas <see cref="Unit"/> está presente.
/// Usada para sinalizar ausência de valor significativo em contextos onde <see cref="Unit"/> é utilizado como substituto de 'void'.
/// </summary>
public class UnitException : Exception
{
    public UnitException () : base("No value present") { }
}