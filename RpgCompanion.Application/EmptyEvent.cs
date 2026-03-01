using RpgCompanion.Core.Events;

namespace RpgCompanion.Application;

internal record struct EmptyEvent : IEvent;

internal record struct EmptyEvent<T> : IEvent<T> where T : IEventProducer;
