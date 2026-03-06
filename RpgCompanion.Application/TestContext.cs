using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Application;

public class TestContext : Context
{
   public IEngine Engine => null!;

   public IEvent Trigger => null!;

   public IReadOnlyList<IObject> Objects => null!;

   public Dictionary<string, dynamic> SharedData { get; set; } = new();
}