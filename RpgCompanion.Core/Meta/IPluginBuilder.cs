using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Core.Engine;

public interface IPluginBuilder
{
    public IPluginBuilder AddEvent<TEvent>(Action<IEventBuilder<TEvent>> configure) where TEvent : IEvent;
    public IPluginBuilder WithMetadata(Action<IPluginMetadataBuilder> configure);
}
