namespace RpgCompanion.Canva.Persistence;

using Core;
using Microsoft.Extensions.Logging;

public static class PersistenceTest
{
    public static async Task Run(IDatabase db, ILogger<Initialization> logger, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Started persistence test");

            var entity = new Entity
            {
                DbId = new DatabaseId<Entity>(),
                NumberValue = 10,
                TextValue = "Entity",
                ComplexValue = new ComplexValue()
                {
                    NumberValue = 20,
                    TextValue = "Complex Value",
                },
            };
            var otherEntity = new OtherEntity
            {
                DbId = new DatabaseId<OtherEntity>(),
                NumberValue = 30,
                TextValue = "Other Entity",
            };
            entity.RelationalValue = Rel.Loaded(otherEntity);

            try
            {
                await db.SaveAsync(entity, cancellationToken);
                logger.LogInformation("Entity saved successfully. Entity id: {0}", entity.DbId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error saving the entity");
                return;
            }

            try
            {
                var entity2 = await db.GetAsync(entity.DbId, cancellationToken);
                if (entity2 is null)
                {
                    logger.LogError("Entity not found");
                    return;
                }
                logger.LogInformation("Entity fetched successfully");
                logger.LogInformation("Entity text value: {0}", entity2.TextValue);
                logger.LogInformation("Complex value: {0}", entity2.ComplexValue.NumberValue);
                logger.LogInformation("Other entity status: {0}", entity2.RelationalValue.GetType().Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error fetching the entity");
            }
        }
        finally
        {
            logger.LogInformation("Finished persistence test");
        }
    }
}
