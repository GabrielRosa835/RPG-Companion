namespace RpgCompanion.Tests.Serialization;

using Core;

// 2. A standard POCO to test the DefaultSerializer fallback
public class SpellPoco : ISerializable<SpellPoco>
{
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public bool RequiresConcentration { get; set; }

    public void Serialize(ISerializationContext context)
        => DefaultSerializer.Serialize(this, context);

    public static SpellPoco Deserialize(IDeserializationContext context) =>
        DefaultSerializer.Deserialize<SpellPoco>(context);
}
