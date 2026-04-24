namespace Utils.Validation;

public class ValidationResult
{
    public List<ValidationError> Errors { get; set; } = new();
    public bool HasErrors => Errors.Count > 0;

    public ValidationResult() { }
    public ValidationResult(IEnumerable<ValidationError> errors)
    {
        Errors.AddRange(errors);
    }
    public ValidationResult(ValidationError first, params ValidationError[] others)
    {
        Errors.Add(first);
        Errors.AddRange(others);
    }
    public ValidationResult(ValidationException exception)
    {
        Errors.AddRange(exception.Errors);
    }

    public void Add(ValidationError error)
    {
        Errors.Add(error);
    }
    public void AddRange(IEnumerable<ValidationError> errors)
    {
        Errors.AddRange(errors);
    }
    public ValidationResult Join(ValidationResult? other)
    {
        if (other is null) return this;
        var result = new ValidationResult(Errors);
        result.Errors.AddRange(other.Errors);
        return result;
    }
}