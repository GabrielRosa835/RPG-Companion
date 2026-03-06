using System.Reflection;

namespace RpgCompanion.Core.Engine;

internal class ContextValidator
{
    private static PropertyInfo? _requirements(Type eventType)
    {
        return typeof(IEventContract<>).MakeGenericType(eventType).GetProperty(nameof(IEventContract<>.Requirements));
    }
    private static PropertyInfo? _outputs(Type eventType)
    {
        return typeof(IEventContract<>).MakeGenericType(eventType).GetProperty(nameof(IEventContract<>.Outputs));
    }
    
    public void ValidateInputs (Context context, object contract, Type eventType)
    {
        IEnumerable<ContextKey>? requirements = (IEnumerable<ContextKey>?) _requirements(eventType)?.GetValue(contract);
        if (requirements is null)
        {
            return;
        }
        foreach (var key in requirements)
        {
            if (!context._data.Contains(key))
            {
                throw new ContractViolationException($"Event '{eventType.Name}' missing input: {key}");
            }
        }
    }

    public void ValidateOutputs (Context context, object contract, Type eventType)
    {
        IEnumerable<ContextKey>? outputs = (IEnumerable<ContextKey>?) _outputs(eventType)?.GetValue(contract);
        if (outputs is null)
        {
            return;
        }
        foreach (var key in outputs)
        {
            if (!context._data.Contains(key))
            {
                throw new ContractViolationException($"Event '{eventType.Name}' failed to produce output: {key}");
            }
        }
    }
}