namespace RpgCompanion.Toolbox;

public record ExceptionDTO
{
    public string Type { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string Details { get; init; } = default!;
    public string StackTrace { get; init; } = default!;
    public List<ExceptionDTO> InnerExceptions { get; } = [];

    public ExceptionDTO() { }
    public ExceptionDTO(Exception e, bool keepStackTrace = true)
    {
        Type = e.GetType().FullName!;
        Message = e.Message;
        Details = e.ToString();
        if (keepStackTrace) StackTrace = e.StackTrace!;
        if (e is AggregateException aggEx)
        {
            foreach (var inner in aggEx.InnerExceptions)
            {
                InnerExceptions.Add(new ExceptionDTO(inner));
            }
        }
        else if (e.InnerException != null)
        {
            InnerExceptions.Add(new ExceptionDTO(e.InnerException));
        }
    }

    public Exception ToException() => new($"[{Type}] {Message}");
}
