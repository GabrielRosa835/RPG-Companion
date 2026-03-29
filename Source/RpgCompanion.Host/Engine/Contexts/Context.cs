namespace RpgCompanion.Application;

using Core.Engine;
using Core.Engine.Contexts;
using Engine.Contexts;
using RpgCompanion.Engine.Components;

// Scoped
internal class Context(
    Registry registry, // Transient
    ContextData data) // Singleton-ish (Metadata)
    : IContext
{
    internal PastEventsCollection InternalPastEvents { get; } = new();
    internal ContextData InternalData => data;
    internal Registry InternalRegistry => registry;

    public IPastEvents PastEvents => this.InternalPastEvents;
    public IContextData Data => this.InternalData;
    public IRegistry Registry => this.InternalRegistry;
}
