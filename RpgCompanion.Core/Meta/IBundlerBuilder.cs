namespace RpgCompanion.Core.Meta;

using Contexts;
using Events;

public interface IBundlerBuilder<TEvent> where TEvent : IEvent
{
    public IBundlerBuilder<TEvent> WithComponent<TBundler>() where TBundler : class, IBundler<TEvent>;
    public IBundlerBuilder<TEvent> WithAction(BundlerAction<TEvent> action);
}
