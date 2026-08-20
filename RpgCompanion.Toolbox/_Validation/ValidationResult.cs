namespace RpgCompanion.Toolbox;

using System.Collections;

public class ValidationResult : IEnumerable<ValidationError>
{
    public List<ValidationError> Errors { get; } = [];
    public bool Failed => Errors.Count > 0;

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
        Errors.AddRange(exception.Result.Errors);
    }

    public ValidationResult Merge(ValidationResult? other)
    {
        if (other is null) return this;
        var result = new ValidationResult(Errors);
        result.Errors.AddRange(other.Errors);
        return result;
    }
    public void Add(ValidationError error)
    {
        Errors.Add(error);
    }
    public void AddRange(IEnumerable<ValidationError> errors)
    {
        Errors.AddRange(errors);
    }

    public IEnumerator<ValidationError> GetEnumerator() => Errors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
