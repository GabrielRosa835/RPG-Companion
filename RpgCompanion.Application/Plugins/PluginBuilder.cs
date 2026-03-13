using RpgCompanion.Application.Services;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;


internal class PluginBuilder : IPluginBuilder
{
   private readonly ComponentCollection _components = new();
   
   internal Type? InitializerType { get; private set; }
   internal Action<IRegistry>? Initialization { get; private set; }
   internal string? PluginName { get; private set; }
   internal string? PluginVersion { get; private set; }

   internal ComponentProvider Build () => _components.BuildProvider();

   public IEventBuilder<TEvent> AddEvent<TEvent> () where TEvent : IEvent
   {
      var eventDescriptor = _components.AddEvent<TEvent>();
      return new EventBuilder<TEvent>(eventDescriptor, _components);
   }
   public IPluginBuilder WithInitializer<TInitializer> () where TInitializer : class, IInitializer
   {
      _components.AddInitializer<TInitializer>();
      InitializerType = typeof(TInitializer);
      return this;
   }
   public IPluginBuilder WithName (string name)
   {
      PluginName = name;
      return this;
   }
   public IPluginBuilder WithVersion (string version)
   {
      PluginVersion = version;
      return this;
   }
   public IPluginBuilder WithInitialization (Action<IRegistry> initialization)
   {
      Initialization = initialization;
      return this;
   }
}
