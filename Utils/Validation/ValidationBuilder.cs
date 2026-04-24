using Utils.UnionTypes;

namespace Utils.Validation;

public class ValidationBuilder
{
    private readonly List<ValidationError> _errors = new();

    public ValidationResult Build()
    {
        return new ValidationResult(_errors);
    }
    public ValidationException BuildException()
    {
        return new ValidationException(_errors);
    }
    public bool TryBuildException(out ValidationException exception)
    {
        if (_errors.Count > 0)
        {
            exception = BuildException();
            return true;
        }
        exception = default!;
        return false;
    }
    public ValidationBuilder ThrowIfHasErrors()
    {
        return _errors.Count > 0 ? throw BuildException() : this;
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
    public ValidationBuilder Join(ValidationException? exception)
    {
        if (exception is not null) _errors.AddRange(exception.Errors);
        return this;
    }
    public ValidationBuilder Join(ValidationResult? result)
    {
        if (result is not null) _errors.AddRange(result.Errors);
        return this;
    }
    public ValidationBuilder Join(ValidationBuilder? other)
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
        if (validator is BaseValidator<T> b)
        {
            b.Validate(item, this);
            return this;
        }
        Attempt result = validator.Validate(item);
        if (result.IsFailure && result.GetFailure() is ValidationException ve)
        {
            return Join(ve);
        }
        return this;
    }
    public ValidationBuilder CascadeForAll<T>(IEnumerable<T> items, IValidator<T> validator)
    {
        T[] array = items as T[] ?? items.ToArray();
        if (validator is BaseValidator<T> b)
        {
            foreach (var item in array)
            {
                b.Validate(item, this);
            }
            return this;
        }
        foreach (var item in array)
        {
            Attempt result = validator.Validate(item);
            if (result.IsFailure && result.GetFailure() is ValidationException ve)
            {
                Join(ve);
            }
        }
        return this;
    }
}