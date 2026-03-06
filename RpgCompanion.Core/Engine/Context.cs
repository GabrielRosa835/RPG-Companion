using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public class Context
{
   private readonly Dictionary<string, dynamic> _data = new();

   public Dictionary<string, dynamic> Data => _data;

   internal bool Contains (ContextKey key) => _data.ContainsKey(key.Name);

   public T Get<T> (ContextKey<T> key) => (T) _data[key.Name];
   public void Set<T> (ContextKey<T> key, T value) => _data[key.Name] = value!;
   public bool Contains<T> (ContextKey<T> key) => _data.ContainsKey(key.Name);
   public bool TryGet<T> (ContextKey<T> key, out T value)
   {
      if (_data.TryGetValue(key.Name, out dynamic v))
      {
         value = (T) v;
         return true;
      }
      value = default!;
      return false;
   }

   public void Raise (IEvent @event)
   {

   }
   public void Invoke (IEvent @event)
   {

   }
}

public abstract record ContextKey (string Name);
public record ContextKey<T> (string Name) : ContextKey(Name);

public interface IEventContract
{
   IEnumerable<ContextKey> Requirements { get; }
   IEnumerable<ContextKey> Outputs { get; }
}

public interface IEventValidator
{
   void ValidateInputs (Context context, IEventContract contract);
   void ValidateOutputs (Context context, IEventContract contract);
}

public class DefaultContextValidator : IEventValidator
{
   public void ValidateInputs (Context context, IEventContract contract)
   {
      foreach (var key in contract.Requirements)
      {
         if (!context.Contains(key))
         {
            throw new ContractViolationException($"Event '{evt.Name}' missing input: {key}");
         }
      }
   }

   public void ValidateOutputs (Context context, IEventContract contract)
   {
      foreach (var key in contract.Outputs)
      {
         if (!context.Contains(key))
         {
            throw new ContractViolationException($"Event '{evt.Name}' failed to produce output: {key}");
         }
      }
   }
}

[Serializable]
internal class ContractViolationException : Exception
{
   public ContractViolationException ()
   {
   }

   public ContractViolationException (string? message) : base(message)
   {
   }

   public ContractViolationException (string? message, Exception? innerException) : base(message, innerException)
   {
   }
}

public interface IContextTemplate
{
   void Bundle (IEvent @event, Context context);
}