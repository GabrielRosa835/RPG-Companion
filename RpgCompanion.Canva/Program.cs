using RpgCompanion.Core.Persistence;
using RpgCompanion.Host.Serialization;

// ==============================================================================
// 1. EXECUTION HARNESS
// ==============================================================================

Console.WriteLine("--- RPG Companion Serialization Engine Test ---");

// Setup the provider (In a real app, this goes in your DI container as a Singleton)
ISerializationProvider provider = new SystemTextJsonSerializationProvider(indented: true);

// Create a mock event that might flow through your MediatR pipeline
var originalEvent = new RuleEvaluatedEvent
{
    EventId = Guid.NewGuid(),
    RuleName = "Sneak Attack",
    IsCritical = true,
    DamageTotal = 42.5m,
    Target = new EntityRef { Id = 104, Name = "Goblin Boss" },
    Tags = ["Combat", "Rogue", "Advantage"]
};

Console.WriteLine("\n[1] Serializing Original Event...");
string jsonOutput = provider.Serialize(originalEvent);
Console.WriteLine(jsonOutput);

Console.WriteLine("\n[2] Deserializing JSON back to Object...");
var reconstructedEvent = provider.Deserialize<RuleEvaluatedEvent>(jsonOutput);

Console.WriteLine("\n[3] Verification:");
Console.WriteLine($"EventId Match: {originalEvent.EventId == reconstructedEvent.EventId}");
Console.WriteLine($"Target Name Match: {originalEvent.Target?.Name == reconstructedEvent.Target?.Name}");
Console.WriteLine($"Tags Count: {reconstructedEvent.Tags.Count} (Expected 3)");
Console.WriteLine($"Damage Value: {reconstructedEvent.DamageTotal}");

// ==============================================================================
// 2. DOMAIN MODELS (The Consumers)
// ==============================================================================

public class EntityRef : ISerializable<EntityRef>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public void Serialize(ISerializationContext context)
    {
        context.Object(c => c
            .Field("id").Number(Id)
            .Field("name").String(Name)
        );
    }

    public static EntityRef Deserialize(IDeserializationContext context)
    {
        return new EntityRef
        {
            Id = context.GetNumber<int>("id"),
            Name = context.GetString("name")
        };
    }
}

public class RuleEvaluatedEvent : ISerializable<RuleEvaluatedEvent>
{
    public Guid EventId { get; set; }
    public string RuleName { get; set; } = string.Empty;
    public bool IsCritical { get; set; }
    public decimal DamageTotal { get; set; }
    public EntityRef? Target { get; set; }
    public List<string> Tags { get; set; } = new();

    public void Serialize(ISerializationContext context)
    {
        context.Object(c =>
        {
            c.Field("eventId").String(EventId.ToString());
            c.Field("ruleName").String(RuleName);
            c.Field("isCritical").Boolean(IsCritical);
            c.Field("damageTotal").Number(DamageTotal);

            c.Field("target");
            if (Target != null) Target.Serialize(c);
            else c.Null();

            c.Field("tags").Array(arr =>
            {
                foreach (var tag in Tags)
                {
                    arr.String(tag);
                }
            });
        });
    }

    public static RuleEvaluatedEvent Deserialize(IDeserializationContext context)
    {
        return new RuleEvaluatedEvent
        {
            EventId = Guid.Parse(context.GetString("eventId")),
            RuleName = context.GetString("ruleName"),
            IsCritical = context.GetBoolean("isCritical"),
            DamageTotal = context.GetNumber<decimal>("damageTotal"),

            Target = context.IsNull("target") ? null : context.GetObject("target", EntityRef.Deserialize),

            Tags = context.GetArray("tags", item => item.GetString()).ToList()
        };
    }
}
