using RpgCompanion.Application.Reflection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Engine;

internal class ContextValidator (Reflect reflect)
{
   private readonly Reflect _reflect = reflect;

   public void ValidateInputs (ComponentDescriptor contract, Context context)
   {
      IEnumerable<ContextKey>? requirements = _reflect
          .ContractRequirements(contract.GenericType)?
          .GetValue(contract)?
          .As<IEnumerable<ContextKey>>();
      if (requirements is null)
      {
         return;
      }
      foreach (var key in requirements)
      {
         if (!context._data.Contains(key))
         {
            throw new ContractViolationException($"Event '{contract.EventType!.Name}' missing input: {key}");
         }
      }
   }

   public void ValidateOutputs (ComponentDescriptor contract, Context context)
   {
      IEnumerable<ContextKey>? outputs = _reflect
          .ContractRequirements(contract.GenericType)?
          .GetValue(contract)?
          .As<IEnumerable<ContextKey>>();
      if (outputs is null)
      {
         return;
      }
      foreach (var key in outputs)
      {
         if (!context._data.Contains(key))
         {
            throw new ContractViolationException($"Event '{contract.EventType!.Name}' failed to produce output: {key}");
         }
      }
   }
}