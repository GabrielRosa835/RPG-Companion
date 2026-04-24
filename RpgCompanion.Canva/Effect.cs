namespace RpgCompanion.Canva;

public interface IEffect<in T, out TResult>
{
    TResult Apply(T current);
}

public delegate TU Effect<in T, out TU>(T current);
