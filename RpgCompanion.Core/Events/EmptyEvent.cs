namespace RpgCompanion.Core.Events;

internal record struct EmptyEvent : IEvent;

internal record struct EmptyEvent<T> : IEvent<T> where T : IEventProducer;
