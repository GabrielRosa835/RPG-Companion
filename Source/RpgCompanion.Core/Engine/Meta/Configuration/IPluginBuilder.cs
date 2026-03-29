using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Meta;

public interface IPluginBuilder
{
    public IPluginBuilder AddEvent<TEvent>(Action<IEventBuilder<TEvent>> configure) where TEvent : IEvent;
    public IPluginBuilder WithMetadata(Action<IPluginMetadataBuilder> configure);
    public IPluginBuilder WithInitialization(Action<IInitializationBuilder> configure);
}
