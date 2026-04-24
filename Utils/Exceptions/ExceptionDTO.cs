using System.Text.Json;

namespace Utils.Exceptions;

public record ExceptionDTO
{
    public string Type { get; init; }
    public string Message { get; init; }
    public string Details { get; init; }
    public string StackTrace { get; init; }
    public List<ExceptionDTO> InnerExceptions { get; set; } = new();

    public ExceptionDTO() { }
    public ExceptionDTO(Exception e)
    {
        Type = e.GetType().FullName;
        Message = e.Message;
        Details = e.ToString();
        StackTrace = e.StackTrace;
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

    public Exception ToException() => new Exception(ToString());

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}