namespace RpgCompanion.Canva.Pipeline;

using Core.Events;

public record ConsoleWrite(string Message) : IEvent;
