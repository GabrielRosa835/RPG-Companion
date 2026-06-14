namespace RpgCompanion.Tests.Serialization;

using Core;

// 4. A POCO that will use an external ISerializer<T> registered in DI
public class MagicItem : ISerializable<MagicItem>
{
    public string ItemName { get; set; } = string.Empty;
    public int RarityTier { get; set; }

    public void Serialize(ISerializationContext context)
    {
        context.Object(c =>
        {
            c.Field("item_name").String(ItemName);
            c.Field("rarity_tier").Number(RarityTier);
        });
    }

    public static MagicItem Deserialize(IDeserializationContext context)
    {
        return new MagicItem
        {
            ItemName = context.GetString("item_name"),
            RarityTier = context.GetNumber<int>("rarity_tier")
        };
    }
}
