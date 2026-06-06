namespace RpgCompanion.Core.Persistence;

using RpgCompanion.Core.Persistence;

public class RuleProcessedEvent : ISerializable<RuleProcessedEvent>
{
    public string RuleName { get; set; } = string.Empty;
    public int TargetEntityId { get; set; }
    public List<string> ModifiersApplied { get; set; } = new();

    public void Serialize(ISerializationContext context)
    {
        // The delegate gracefully scopes the root object
        context.Object(c => c
            .Field("ruleName").String(RuleName)
            .Field("targetEntityId").Number(TargetEntityId)
            .Field("modifiersApplied").Array(arr =>
            {
                foreach (var mod in ModifiersApplied)
                {
                    arr.String(mod);
                }
            })
        );
    }

    public static RuleProcessedEvent Deserialize(IDeserializationContext context)
    {
        return new RuleProcessedEvent
        {
            RuleName = context.GetString("ruleName"),
            TargetEntityId = context.GetNumber<int>("targetEntityId"),

            // The Func scope elegantly pulls out the array elements
            ModifiersApplied = context.GetArray("modifiersApplied", itemContext =>
                itemContext.GetString("$value") // "$value" or similar convention for array primitives
            ).ToList()
        };
    }
}
