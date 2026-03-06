namespace RpgCompanion.Core.Events;

public interface IEvent
{
   string Name { get; }
}

public record struct EmptyEvent : IEvent
{
   public string Name => nameof(EmptyEvent);
}