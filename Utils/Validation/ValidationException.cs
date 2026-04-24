namespace Utils.Validation;

public class ValidationException : Exception
{
    private const string DEFAULT_MESSAGE = "One or more validation errors occurred.";
    private readonly List<ValidationError> _errors = new();

    public IEnumerable<ValidationError> Errors => _errors;
    public bool HasErrors => _errors.Count > 0;

    public ValidationException(IEnumerable<ValidationError> errors)
    {
        _errors.AddRange(errors);
    }
    public ValidationException(ValidationError first, params ValidationError[] others)
    {
        _errors.Add(first);
        _errors.AddRange(others);
    }
    public ValidationException(ValidationResult result)
    {
        _errors.AddRange(result.Errors);
    }
    public ValidationException() : base(DEFAULT_MESSAGE) {}
    public ValidationException(string message) : base(message) {}
    public ValidationException(string message, Exception innerException) : base(message, innerException) {}

    public void Add(ValidationError error)
    {
        _errors.Add(error);
    }
    public void AddRange(IEnumerable<ValidationError> errors)
    {
        _errors.AddRange(errors);
    }
    public void Join(ValidationException? other)
    {
        if (other is not null)
        {
            _errors.AddRange(other._errors);
        }
    }
}