using RpgCompanion.Application.Services;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

public static class Canva
{
   public record TestPlugin (string Id, string Name, string Version) : IPlugin;
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
         .AddRule(rule1)
         .WithPlacement(RulePlacement.AfterEffect)
         .Return()
         .AddRule(rule2)
         .WithPlacement(RulePlacement.BeforeEffect)
         .Return()
         .AddEffect(effect1)
         .WithPriority(1)
         .Return();
   }

   public record Event () : EventBase(nameof(Event));
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
   public RuleBuilder<TRule, TEvent> AddRule<TRule>() where TRule : IRule<TEvent>
   {
      var descriptor = components.AddRule<TRule, TEvent>();
      return new RuleBuilder<TRule, TEvent>(this, descriptor);  
   }
   public RuleBuilder<TRule, TEvent> AddRule <TRule>(TRule rule) where TRule : IRule<TEvent>
   {
      var descriptor = components.AddRule<TRule, TEvent>(rule);
      return new RuleBuilder<TRule, TEvent>(this, descriptor);
   }

   public EffectBuilder<TEffect, TEvent> AddEffect<TEffect> () where TEffect : IEffect<TEvent>
   {
      var descriptor = components.AddEffect<TEffect, TEvent>();
      return new EffectBuilder<TEffect, TEvent>(this, descriptor);
   }
   public EffectBuilder<TEffect, TEvent> AddEffect<TEffect> (TEffect effect) where TEffect : IEffect<TEvent>
   {
      var descriptor = components.AddEffect<TEffect, TEvent>(effect);
      return new EffectBuilder<TEffect, TEvent>(this, descriptor);
   }
}

internal class RuleBuilder<TRule, TEvent> (EventBuilder<TEvent> previous, ComponentDescriptor descriptor) 
   where TEvent : IEvent
   where TRule : IRule<TEvent>
{
   public EventBuilder<TEvent> Return () => previous;
   public RuleBuilder<TRule, TEvent> WithPlacement (RulePlacement placement)
   {
      descriptor.Rule_Placement = placement;
      return this;
   }
}

internal class EffectBuilder<TEffect, TEvent> (EventBuilder<TEvent> previous, ComponentDescriptor descriptor)
   where TEvent : IEvent
   where TEffect : IEffect<TEvent>
{
   public EventBuilder<TEvent> Return () => previous;
   public EffectBuilder<TEffect, TEvent> WithPriority (EffectPriority priority)
   {
      descriptor.Effect_Priority = priority.Value();
      return this;
   }
   public EffectBuilder<TEffect, TEvent> WithPriority (int priority)
   {
      descriptor.Effect_Priority = priority;
      return this;
   }
}