namespace RpgCompanion.Tests.Serialization;

using RpgCompanion.Core;

public class PlayerCharacter : ISerializable<PlayerCharacter>
{
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public CharacterStats Stats { get; set; } = new();

    public void Serialize(ISerializationContext context)
    {
        context.Object(c =>
        {
            c.Field("Name").String(Name);
            c.Field("Level").Number(Level);
            c.Field("Stats").Auto(Stats);
        });
    }

    public static PlayerCharacter Deserialize(IDeserializationContext context)
    {
        return new PlayerCharacter
        {
            Name = context.GetString("Name"),
            Level = context.GetNumber<int>("Level"),
            Stats = context.Get
        };
    }
}
