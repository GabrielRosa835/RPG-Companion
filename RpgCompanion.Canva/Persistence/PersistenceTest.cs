// namespace RpgCompanion.Canva.Persistence;
//
// using Core;
// using Microsoft.Extensions.Logging;
//
// public static class PersistenceTest
// {
//     private static readonly Guid EntityFixedGuid = Guid.ParseExact("019fb072d48c77ebbb1f238d328577cd", "N");
//     private static readonly Guid OtherFixedGuid = Guid.ParseExact("019fb072d48d762e832ead1bf9037ab4", "N");
//
//     public static async Task Run(IDatabase db, ILogger<Initialization> logger, CancellationToken cancellationToken)
//     {
//         try
//         {
//             logger.LogInformation("Started persistence test");
//
//             var entity = new Entity
//             {
//                 Id = DatabaseId.Create<Entity>(EntityFixedGuid),
//                 NumberValue = 10,
//                 TextValue = "Entity",
//                 ComplexValue = new ComplexValue
//                 {
//                     NumberValue = 20,
//                     TextValue = "Complex Value",
//                 },
//             };
//             var otherEntity = new OtherEntity
//             {
//                 Id = DatabaseId.Create<OtherEntity>(OtherFixedGuid),
//                 NumberValue = 30,
//                 TextValue = "Other Entity",
//             };
//             entity.RelationalValue = Rel.Loaded(otherEntity);
//
//             try
//             {
//                 await db.SaveAsync(entity, cancellationToken);
//                 await db.SaveAsync(otherEntity, cancellationToken);
//                 logger.LogInformation("Entity saved successfully. Entity id: {0}", entity.DbId);
//             }
//             catch (Exception ex)
//             {
//                 logger.LogError(ex, "There was an error saving the entity");
//                 return;
//             }
//
//             try
//             {
//                 var entity2 = await db.GetAsync(entity.Id, cancellationToken);
//                 if (entity2 is null)
//                 {
//                     logger.LogError("Entity not found");
//                     return;
//                 }
//                 logger.LogInformation("Entity fetched successfully");
//                 logger.LogInformation("Entity text value: {0}", entity2.TextValue);
//                 logger.LogInformation("Complex value: {0}", entity2.ComplexValue.NumberValue);
//                 logger.LogInformation("Other entity status: {0}", entity2.RelationalValue.GetType().Name);
//             }
//             catch (Exception ex)
//             {
//                 logger.LogError(ex, "There was an error fetching the entity");
//             }
//         }
//         finally
//         {
//             logger.LogInformation("Finished persistence test");
//         }
//     }
// }
