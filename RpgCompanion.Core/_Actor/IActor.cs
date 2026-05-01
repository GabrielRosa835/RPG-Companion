namespace RpgCompanion.Core;

public interface IActor;

// public static class IActorExtensions
// {
//     private static Dictionary<IActor, Dictionary<string, dynamic>> _extraFields = new();
//
//     extension (IActor actor)
//     {
//         public T Get<T>(ProperyKey<T> key)
//         {
//             lock (_extraFields)
//             {
//                 if (!_extraFields.TryGetValue(actor, out var fields))
//                     throw new KeyNotFoundException($"Missing key {key.Content}");
//
//                 if (fields.TryGetValue(key.Content, out var field))
//                 {
//                     return (T) field;
//                 }
//             }
//             throw new KeyNotFoundException($"Key {key} not found");
//         }
//     }
//
//     public static void Canva(IActor actor)
//     {
//         actor.Get()
//     }
// }
//
// public readonly record struct ProperyKey<T>(string Content);
