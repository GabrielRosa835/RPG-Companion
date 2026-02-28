namespace RpgCompanion.Canva;

public interface IEngine
{
   IContextProvider ContextProvider { get; }
   IEventQueue EventQueue { get; }
   IRegistry Registry { get; }
}

public interface IBackgroundWorker;

public class Engine : IEngine, IBackgroundWorker
{
   private readonly IContextProvider _contextProvider;

   public Engine (IContextProvider contextProvider)
   {
      _contextProvider = contextProvider;
   }

   private readonly IEventQueue _eventQueue;
   private readonly IRegistry _registry;

   public IContextProvider ContextProvider => _contextProvider;
   public IEventQueue EventQueue => _eventQueue;
   public IRegistry Registry => _registry;

   public async Task ExecuteLoop()
   {
      while(!_eventQueue.Any())
      {
         await Task.Delay(100);
      }

      IEvent @event = _eventQueue.Pop();
      IContext context = _contextProvider.Bundle(@event);

      IEnumerable<IPrecondition> preconditions = _registry.Get<IEnumerable<IPrecondition>>()!;
      foreach (IPrecondition condition in preconditions)
      {
         condition.Apply(@event, context);
      }

      IEnumerable<IRule> rules = _registry.Get<IEnumerable<IRule>>()!;
      foreach (IRule rule in rules)
      {
         rule.Apply(context);
      }
   }
}