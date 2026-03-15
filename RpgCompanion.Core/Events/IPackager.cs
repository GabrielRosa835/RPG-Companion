namespace RpgCompanion.Core.Contexts;

using RpgCompanion.Core.Events;

public interface IPackager<in TEvent> where TEvent : IEvent
{
    public void Pack(TEvent e, IEditableContext context);
}
