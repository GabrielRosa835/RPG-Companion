namespace RpgCompanion.Toolbox;

public enum ValidationSeverity
{
    Error,
    Warning,
    Info,
}

public record ValidationError(
    string Message,
    string? Property = null,
    object? Attempted = null,
    object? State = null,
    string? code = null,
    ValidationSeverity Severity = ValidationSeverity.Error)
{
    public override string ToString() => Property is not null ? $"{Property}: {Message}" : Message;
}
