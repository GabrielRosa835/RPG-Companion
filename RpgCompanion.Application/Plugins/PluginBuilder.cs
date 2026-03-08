using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

public static class Canva
{
   public record TestPlugin (string Id, string Name, string Version) : IPlugin;
   public record TestRule : IRule<Event>
   {
      public Event Apply (IContextSnapshot context)
      {
         throw new NotImplementedException();
      }
   }

   public record Event () : EventBase(nameof(Event));
   public static void Test()
   {
      var plugin = new TestPlugin("", "", "");
      PluginBuilder builder = new PluginBuilder(new ComponentCollection(plugin));

      var rule1 = IRule<Event>.Of(c => default!);
      var rule2 = IRule<Event>.Of(c => default!);
      var rule3 = IRule<Event>.Of(c => default!);
      var rule4 = IRule<Event>.Of(c => default!);

      var effect1 = IEffect<Event>.Of((e, c) => { });
      var effect2 = IEffect<Event>.Of((e, c) => { });
      var effect3 = IEffect<Event>.Of((e, c) => { });
      var effect4 = IEffect<Event>.Of((e, c) => { });

      builder.On<Event>()
         .AddRule<TestRule>()

   }
}

internal class PluginBuilder (ComponentCollection components)
{
   internal ComponentProvider Build () => components.BuildProvider();

   internal EventBuilder<TEvent> On<TEvent>() where TEvent : IEvent
   {
      return new EventBuilder<TEvent>(components);
   }
}

internal class EventBuilder<TEvent>(ComponentCollection components) where TEvent : IEvent
{
   public RuleBuilder<TRule, TEvent> AddRule<TRule>(TRule? rule = default) where TRule : IRule<TEvent>
   {
      return new RuleBuilder<TRule, TEvent>(this, components, rule);  
   }
}

internal class RuleBuilder<TRule, TEvent> (
   EventBuilder<TEvent> previous,
   ComponentCollection components,
   TRule? instance = default) 
   where TEvent : IEvent
   where TRule : IRule<TEvent>
{
   private RulePlacement _placement = RulePlacement.AfterEvent;

   public EventBuilder<TEvent> Return () 
   {
      components.AddRule(typeof(TRule), typeof(TEvent), typeof(IRule<TEvent>), _placement, instance);
      return previous;
   }
   public RuleBuilder<TRule, TEvent> AfterEvent ()
   {
      _placement = RulePlacement.AfterEvent;
      return this;
   }
   public RuleBuilder<TRule, TEvent> BeforeEvent ()
   {
      _placement = RulePlacement.BeforeEvent;
      return this;
   }
}