using RpgCompanion.Application.Services;

namespace RpgCompanion.Application.Engines;

internal class EffectQueue
{
   internal readonly PriorityQueue<ComponentDescriptor, int> _effects;

   public EffectQueue (IEnumerable<ComponentDescriptor> values)
   {
      _effects = new(values.Select(v => (v, int.MaxValue - v.Effect_Priority!.Value)));
   }

   public int Count => _effects.Count;

   public ComponentDescriptor Dequeue ()
   {
      return _effects.Dequeue(); 
   }
   public ComponentDescriptor? FirstOrDefault ()
   {
      if (_effects.TryPeek(out var descriptor, out int _))
      {
         return descriptor;
      }
      return null;
   }
}
