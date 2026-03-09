using RpgCompanion.Application.Services;

using System.Collections;

namespace RpgCompanion.Application.Engines;

internal class EffectQueue : IEnumerable<ComponentDescriptor>
{
   internal readonly PriorityQueue<ComponentDescriptor, int> _effects;

   internal EffectQueue (IEnumerable<ComponentDescriptor> values)
   {
      _effects = new(values.Select(v => (v, int.MaxValue - v.Effect_Priority!.Value)));
   }

   internal int Count => _effects.Count;

   internal void SetValues(IEnumerable<ComponentDescriptor> values)
   {
      _effects.Clear();
      _effects.EnqueueRange(values.Select(v => (v, int.MaxValue - v.Effect_Priority!.Value)));
   }
   internal void Clear ()
   {
      _effects.Clear();
   }
   internal ComponentDescriptor Dequeue ()
   {
      return _effects.Dequeue(); 
   }
   internal ComponentDescriptor? FirstOrDefault ()
   {
      if (_effects.TryPeek(out var descriptor, out int _))
      {
         return descriptor;
      }
      return null;
   }

   public IEnumerator<ComponentDescriptor> GetEnumerator ()
   {
      throw new NotImplementedException();
   }

   IEnumerator IEnumerable.GetEnumerator ()
   {
      return GetEnumerator();
   }

   private class Enumerator (PriorityQueue<ComponentDescriptor, int> values) : IEnumerator<ComponentDescriptor>
   {
      private readonly PriorityQueue<ComponentDescriptor, int> _values = new(values);

      public ComponentDescriptor Current => values.Peek();
      object IEnumerator.Current => Current;

      public void Dispose ()
      {
      }

      public bool MoveNext ()
      {
         if (values.Count > 0)
         {
            values.Dequeue();
            return true;
         }
         return false;
      }

      public void Reset ()
      {
         return
      }
   }
}
