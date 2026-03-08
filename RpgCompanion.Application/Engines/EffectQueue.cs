using System.Collections;

namespace RpgCompanion.Application.Engines;

internal class EffectQueue
{
   internal readonly PriorityQueue<object, int> _effects;

   public EffectQueue (IEnumerable<(object Effect, int Priority)> values)
   {
      _effects = new(values.Select(v => (v.Effect, int.MaxValue - v.Priority)));
   }

   public int Count => _effects.Count;

   public object Dequeue()
   {
      return _effects.Dequeue(); 
   }
}
