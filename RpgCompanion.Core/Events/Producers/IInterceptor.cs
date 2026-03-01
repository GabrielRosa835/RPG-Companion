using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Utils;

namespace RpgCompanion.Core.Events.Producers;

public interface IInterceptor
{
   IEvent Apply (IEvent @event, IContext context);
}

public interface IInterceptor<in TEventIn> : IInterceptor
   where TEventIn : IEvent
{
   IEvent Apply (TEventIn @event, IContext context);
   IEvent IInterceptor.Apply (IEvent @event, IContext context) => Apply(@event.As<TEventIn>(), context);
}

public interface IInterceptor<in TEventIn, out TEventOut> : IInterceptor<TEventIn>, IEventProducer
   where TEventIn : IEvent
   where TEventOut : IEvent<IInterceptor<TEventIn, TEventOut>>
{
   new TEventOut Apply (TEventIn @event, IContext context);
   IEvent IInterceptor<TEventIn>.Apply (TEventIn @event, IContext context) => Apply(@event, context).As<TEventOut>();
}