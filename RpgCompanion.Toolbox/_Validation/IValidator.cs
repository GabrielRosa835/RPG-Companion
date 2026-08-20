namespace RpgCompanion.Toolbox;

public interface IValidator<in T>
{
    Attempt Validate(T item);
}
