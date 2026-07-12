namespace RpgCompanion.Core;

public abstract record Action;

public abstract record Reaction(EventDescriptor ForEvent, ActionTiming Timing);

public class ActionContext
{
}

public enum ActionTiming
{
    BeforeEvent = 0,
    EventStarted = 1,
    EventFinished = 2,
    AfterEvent = 3,
}
