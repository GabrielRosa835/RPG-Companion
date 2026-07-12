// namespace RpgCompanion.DnD;
//
// public class AttackIntent : IPlayerIntent
// {
//     public string AttackerId { get; set; }
//     public string TargetId { get; set; }
//     public string WeaponId { get; set; }
// }
//
// public class AttackResult
// {
//     public bool IsHit { get; set; }
//     public int Damage { get; set; }
// }
//
// public class AttackIntentHandler : IIntentHandler<AttackIntent, AttackResult>
// {
//     public async Task<AttackResult> HandleAsync(AttackIntent intent)
//     {
//         // 1. Fetch actors from persistence (abstracted by the host)
//         // 2. Feed data into your existing StateMachine/Rule Engine
//         // 3. Return the result
//         return new AttackResult { IsHit = true, Damage = 12 };
//     }
// }
//
// // Inside the plugin's IManifest implementation:
// public void Configure(IPluginConfiguration plugin)
// {
//     plugin.AddIntent<AttackIntent, AttackIntentHandler, AttackResult>();
// }
