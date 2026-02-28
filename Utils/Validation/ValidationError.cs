namespace Utils.Validation;

public record ValidationError(string FieldName, string ErrorMessage, object? AttemptedValue = null)
{
    public override string ToString() => $"{FieldName}: {ErrorMessage}";
}