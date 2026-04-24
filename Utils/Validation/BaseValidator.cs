using Utils.UnionTypes;

namespace Utils.Validation;

public abstract class BaseValidator<T> : IValidator<T>
{
    public Attempt Validate(T item)
    {
        ValidationBuilder builder = new();
        Validate(item, builder);
        return builder.TryBuildException(out var exception)
            ? Results.Failure(exception)
            : Results.Success();
    }

    public abstract void Validate(T item, ValidationBuilder builder);
}