using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Events.Producers;

public interface IRule<out TEvent> : IEventProducer where TEvent : IEvent<IRule<TEvent>>
{
   TEvent Handle (Context context);
   public static IRule<TEvent> Of (Func<Context, TEvent> function) => new RuleDelegate<TEvent>(function);
}

internal class RuleDelegate<TEvent> (Func<Context, TEvent> function) : IRule<TEvent> where TEvent : IEvent<IRule<TEvent>>
{
   public TEvent Handle (Context context) => function(context);
}
