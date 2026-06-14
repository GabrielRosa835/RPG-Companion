namespace RpgCompanion.Tests.Serialization;

using Core;

public class CharacterStats : ISerializable<CharacterStats>
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }

    public void Serialize(ISerializationContext context)
    {
        context.Object(c =>
        {
            c.Field("str").Number(Strength);
            c.Field("dex").Number(Dexterity);
        });
    }

    public static CharacterStats Deserialize(IDeserializationContext context)
    {
        return new CharacterStats
        {
            Strength = context.GetNumber<int>("str"),
            Dexterity = context.GetNumber<int>("dex")
        };
    }
}
