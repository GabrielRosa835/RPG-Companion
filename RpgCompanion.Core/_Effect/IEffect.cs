namespace RpgCompanion.Core;

public interface IEffect<in T, out TResult>
{
    TResult Apply(T current);
}
