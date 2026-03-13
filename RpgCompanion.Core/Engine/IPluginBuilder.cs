using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Core.Engine;

public interface IPluginBuilder
{
    public IEventBuilder<TEvent> AddEvent<TEvent>() where TEvent : IEvent;
    public IPluginBuilder WithInitializer<TInitializer>() where TInitializer : class, IInitializer;
    public IPluginBuilder WithName(string name);
    public IPluginBuilder WithVersion(string version);
    public IPluginBuilder WithInitialization(Action<IRegistry> initialization);
}
