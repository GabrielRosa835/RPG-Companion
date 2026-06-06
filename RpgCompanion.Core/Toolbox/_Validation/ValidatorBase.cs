namespace RpgCompanion.Core.Toolbox;

public abstract class ValidatorBase<T> : IValidator<T>
{
    public Attempt Validate(T item)
    {
        ValidationBuilder builder = new();
        Validate(item, builder);
        return builder.TryBuildFailure(out var result)
            ? Results.Failure(new ValidationException(result))
            : Results.Success();
    }

    public abstract void Validate(T item, ValidationBuilder builder);
}
