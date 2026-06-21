namespace RpgCompanion.Host;

internal interface IEventPublisher
{
    void Publish(EventContext context);
}
