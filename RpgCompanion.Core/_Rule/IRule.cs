namespace RpgCompanion.Core;

public interface IRule<T>
{
    T Apply(T current);
}
