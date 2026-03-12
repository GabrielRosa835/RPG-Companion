using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application;

public static class EventPriorities
{
   public const int None = 0;
   public const int Low = 100;
   public const int Medium = 200;
   public const int High = 300;
}

internal class EventBuilder<TEvent> (EventDescriptor eventDescriptor, ComponentCollection components) where TEvent : IEvent
{
   public EventBuilder<TEvent> WithName (string name)
   {
      eventDescriptor.DisplayName = name;
      return this;
   }
   public EventBuilder<TEvent> WithPriority (int priority)
   {
      eventDescriptor.Priority = priority;
      return this;
   }
   public EventComponentBuilder<TEvent> WithComponents()
   {
      return new EventComponentBuilder<TEvent>(components);
   }
}

internal class EventComponentBuilder<TEvent>(ComponentCollection components) where TEvent : IEvent
{
   public EventComponentBuilder<TEvent> AddRule<TRule> () where TRule : IRule<TEvent>
   {
      components.AddRule<TRule, TEvent>();
      return this;
   }
   public EventComponentBuilder<TEvent> AddEffect<TEffect> () where TEffect : IEffect<TEvent>
   {
      components.AddEffect<TEffect, TEvent>();
      return this;
   }
   public EventComponentBuilder<TEvent> AddContract<TContract> () where TContract : IContract<TEvent>
   {
      components.AddContract<TContract, TEvent>();
      return this;
   }
   public EventComponentBuilder<TEvent> AddPackager<TPackager> () where TPackager : IPackager<TEvent>
   {
      components.AddPackager<TPackager, TEvent>();
      return this;
   }
}