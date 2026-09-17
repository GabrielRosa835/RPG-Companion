namespace RpgCompanion.Toolbox.Validation;

public abstract class ValidatorBase<T> : IValidator<T>
{
    public Attempt<T, ValidationResult> Validate(T item)
    {
        ValidationBuilder builder = new();
        Validate(item, builder);
        return builder.TryBuildFailure(out var result)
            ? result
            : Results.Success<T, ValidationResult>(item);
    }

    public abstract void Validate(T item, ValidationBuilder builder);
}
