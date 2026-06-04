namespace RpgCompanion.Core;

public enum ActorLifetime
{
    Persistent, // Singleton
    Temporary, // Scoped
    Immediate, // Transient
}
