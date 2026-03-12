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
   public record Event : IEvent;
   public record TestInitializer : EmptyInitializer;
   public static void Test ()
   {
      PluginBuilder builder = new PluginBuilder();

      builder.AddEvent<Event>()
         .WithName(nameof(Event))
         .WithPriority(1)
         .WithPriority(EventPriorities.High)
         .WithComponents()
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

   internal EventBuilder<TEvent> AddEvent<TEvent> () where TEvent : IEvent
   {
      var eventDescriptor = _components.AddEvent<TEvent>();
      return new EventBuilder<TEvent>(eventDescriptor, _components);
   }

   internal Type? InitializerType { get; set; }
   internal Action<IRegistry>? Initialization { get; set; }
   internal string? PluginName { get; set; }
   internal string? PluginVersion { get; set; }

   public PluginBuilder WithInitializer<TInitializer> ()
   {
      InitializerType = typeof(TInitializer);
      return this;
   }
   public PluginBuilder WithName (string name)
   {
      PluginName = name;
      return this;
   }
   public PluginBuilder WithVersion (string version)
   {
      PluginVersion = version;
      return this;
   }

   public PluginBuilder WithInitialization (Action<IRegistry> initialization)
   {
      Initialization = initialization;
      return this;
   }
}
