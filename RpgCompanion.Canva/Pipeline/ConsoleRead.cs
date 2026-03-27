namespace RpgCompanion.Canva.Pipeline;

using Core.Events;

public class ConsoleRead : IEvent
{
    public string Input { get; set; }
}
