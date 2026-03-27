namespace RpgCompanion.Canva.Pipeline;

using Core.Events;

public record ConsoleWriteLine(string message) : IEvent
{
    public string Message { get; init; } = message;
}
