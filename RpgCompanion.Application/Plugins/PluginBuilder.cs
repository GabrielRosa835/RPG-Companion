using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

public static class Canva
{
   public record TestRule : EmptyRule<Event>;
   public record TestEffect : EmptyEffect<Event>;
   public record TestContract : EmptyContract<Event>;
   public record TestPackager : EmptyPackager<Event>;
   public record Event : EmptyEvent;
   public record TestInitializer : EmptyInitializer;
   public static void Test()
   {
      PluginBuilder builder = new PluginBuilder();

      builder.For<Event>()
         .AddRule<TestRule>()
         .AddEffect<TestEffect>()
         .AddContract<TestContract>()
         .AddPackager<TestPackager>();
      builder.WithInitialization(r => { })
         .WithInitializer<TestInitializer>()
         .WithName("Test")
         .WithVersion("1.0.0");
   }
}

internal class PluginBuilder
{
   private readonly ComponentCollection _components = new();
      
   internal ComponentProvider Build () => _components.BuildProvider();

   internal EventBuilder<TEvent> For<TEvent>() where TEvent : IEvent
   {
      return new EventBuilder<TEvent>(_components, this);
   }

   private Type? _initializerType;
   private Action<IRegistry>? _initialization;
   private string? _pluginName;
   private string? _pluginVersion;
   
   public PluginBuilder WithInitializer<TInitializer>()
   {
      _initializerType = typeof(TInitializer);
      return this;
   }
   public PluginBuilder WithName(string name)
   {
      _pluginName = name;
      return this;
   }
   public PluginBuilder WithVersion(string version)
   {
      _pluginVersion = version;
      return this;
   }

   public PluginBuilder WithInitialization(Action<IRegistry> initialization)
   {
      _initialization = initialization;
      return this;
   }
}
