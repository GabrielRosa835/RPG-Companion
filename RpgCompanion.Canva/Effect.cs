namespace RpgCompanion.Canva;

public interface IEffect<in T, out TResult>
{
    TResult Apply(T current);
}

public delegate TResult Effect<in T, out TResult>(T current);
