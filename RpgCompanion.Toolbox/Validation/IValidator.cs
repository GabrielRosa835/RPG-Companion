namespace RpgCompanion.Toolbox.Validation;

public interface IValidator<T>
{
    Attempt<T, ValidationResult> Validate(T item);
}
