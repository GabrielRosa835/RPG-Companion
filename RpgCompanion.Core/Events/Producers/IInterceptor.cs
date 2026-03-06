using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Utils;

namespace RpgCompanion.Core.Events.Producers;

public interface IInterceptor
{
   IEvent Apply (IEvent @event, Context context);
}

public interface IInterceptor<in TEventIn> : IInterceptor
   where TEventIn : IEvent
{
   IEvent Apply (TEventIn @event, Context context);
   IEvent IInterceptor.Apply (IEvent @event, Context context) => Apply(@event.As<TEventIn>(), context);
}

public interface IInterceptor<in TEventIn, out TEventOut> : IInterceptor<TEventIn>, IEventProducer
   where TEventIn : IEvent
   where TEventOut : IEvent<IInterceptor<TEventIn, TEventOut>>
{
   new TEventOut Apply (TEventIn @event, Context context);
   IEvent IInterceptor<TEventIn>.Apply (TEventIn @event, Context context) => Apply(@event, context).As<TEventOut>();

   public static IInterceptor<TEventIn, TEventOut> Of (Func<TEventIn, Context, TEventOut> function) 
      => new InterceptorDelegate<TEventIn, TEventOut>(function);
}

public sealed class InterceptorDelegate<TEventIn, TEventOut> (Func<TEventIn, Context, TEventOut> function) 
   : IInterceptor<TEventIn, TEventOut> where TEventIn : IEvent
   where TEventOut : IEvent<IInterceptor<TEventIn, TEventOut>>
{
   public TEventOut Apply (TEventIn @event, Context context) => function(@event, context);
}