namespace RpgCompanion.Core.Contexts;

using Events;

public delegate void BundlerAction<in TEvent>(TEvent e, IEditableContext context) where TEvent : IEvent;

public interface IBundler<in TEvent> where TEvent : IEvent
{
    public void Bundle(TEvent e, IEditableContext context);
}
