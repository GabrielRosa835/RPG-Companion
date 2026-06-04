namespace RpgCompanion.Host;

using System.Text.Json;
using RpgCompanion.Core;

internal record EventRaisedEvent
{
    public Guid Guid { get; init; } = Guid.CreateVersion7();
    public JsonElement Data { get; init; }
    public EventKey Key { get; init; }
    public List<Transition> Transitions { get; init; } = [];
}

internal record Transition
{
    public RuleKey Key { get; init; }
    public List<Transition> Chain { get; init; } = [];
}
