namespace RpgCompanion.Canva;

public interface IRule<T>
{
    T Apply(T current);
}

public delegate T Rule<T>(T current);
