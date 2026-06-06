namespace RpgCompanion.Core.Toolbox;

public class ValidationBuilder
{
    private readonly List<ValidationError> _errors = [];

    public ValidationResult Build()
    {
        return new ValidationResult(_errors);
    }

    public bool TryBuildFailure(out ValidationResult result)
    {
        if (_errors.Count > 0)
        {
            result = Build();
            return true;
        }
        result = default!;
        return false;
    }

    public ValidationBuilder ThrowIfFailed()
    {
        return TryBuildFailure(out var result) ? throw new ValidationException(result) : this;
    }

    public ValidationBuilder AddError(string fieldName, string errorMessage, object attemptedValue)
    {
        _errors.Add(new ValidationError(fieldName, errorMessage, attemptedValue));
        return this;
    }

    public ValidationBuilder AddErrors(IEnumerable<ValidationError>? errors)
    {
        if (errors is not null) _errors.AddRange(errors);
        return this;
    }

    public ValidationBuilder AddErrors(params ValidationError[]? errors)
    {
        if (errors is not null) _errors.AddRange(errors);
        return this;
    }

    public ValidationBuilder Merge(ValidationException? exception)
    {
        if (exception is not null) _errors.AddRange(exception.Result.Errors);
        return this;
    }

    public ValidationBuilder Merge(ValidationResult? result)
    {
        if (result is not null) _errors.AddRange(result.Errors);
        return this;
    }

    public ValidationBuilder Merge(ValidationBuilder? other)
    {
        if (other is not null) _errors.AddRange(other._errors);
        return this;
    }

    public ValidationBuilder Clear()
    {
        _errors.Clear();
        return this;
    }

    public ValidationBuilder If(bool condition, string fieldName, string errorMessage, object attemptedValue)
    {
        if (condition)
        {
            AddError(fieldName, errorMessage, attemptedValue);
        }
        return this;
    }

    public ValidationBuilder If(bool condition, Action<ValidationBuilder> action)
    {
        if (condition)
        {
            action(this);
        }
        return this;
    }

    public ValidationBuilder IfNot(bool condition, string fieldName, string errorMessage, object attemptedValue)
    {
        if (!condition)
        {
            AddError(fieldName, errorMessage, attemptedValue);
        }
        return this;
    }

    public ValidationBuilder IfNot(bool condition, Action<ValidationBuilder> action)
    {
        if (!condition)
        {
            action(this);
        }
        return this;
    }

    public ValidationBuilder Cascade<T>(T item, IValidator<T> validator)
    {
        if (validator is ValidatorBase<T> b)
        {
            b.Validate(item, this);
            return this;
        }
        Attempt cascadingResult = validator.Validate(item);
        if (cascadingResult.TryGetFailure(out var failure) && failure is ValidationException ve)
        {
            return Merge(ve);
        }
        return this;
    }

    public ValidationBuilder CascadeForAll<T>(IEnumerable<T> items, IValidator<T> validator)
    {
        if (validator is ValidatorBase<T> b)
        {
            foreach (var item in items)
            {
                b.Validate(item, this);
            }
            return this;
        }
        foreach (var item in items)
        {
            Attempt cascadingResult = validator.Validate(item);
            if (cascadingResult.TryGetFailure(out var failure) && failure is ValidationException ve)
            {
                Merge(ve);
            }
        }
        return this;
    }
}
