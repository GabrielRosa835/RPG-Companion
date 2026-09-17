namespace RpgCompanion.Toolbox;

/// <summary>
/// Exceção lançada quando uma operação espera um valor, mas apenas <see cref="None"/> está presente.
/// Usada para sinalizar ausência de valor significativo em contextos onde <see cref="None"/> é utilizado como substituto de 'void'.
/// </summary>
public class EmptyException : Exception
{
    public EmptyException () : base("No value present") { }
}
