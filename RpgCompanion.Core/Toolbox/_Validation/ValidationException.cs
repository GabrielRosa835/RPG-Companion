namespace RpgCompanion.Core.Toolbox;

public class ValidationException : Exception
{
    private const string DEFAULT_MESSAGE = "One or more validation errors occurred.";

    public ValidationResult Result { get; }

    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Result = new(errors);
    }
    public ValidationException(ValidationError first, params ValidationError[] others)
    {
        Result = new(first, others);
    }
    public ValidationException(ValidationResult result) : base(DEFAULT_MESSAGE)
    {
        Result = result;
    }
    public ValidationException(string message, ValidationResult result) : base(message)
    {
        Result = result;
    }
    public ValidationException(string message, Exception innerException, ValidationResult result) : base(message, innerException)
    {
        Result = result;
    }
}
