namespace RpgCompanion.QuartzPrototype;

using Core.Events;

internal static class Keys
{
    public static JobKey ToJobKey(this IEvent e)
    {
        return new JobKey(e.GetType().Name, e.GetType().Namespace);
    }

    public static TriggerKey ToTriggerKey(this IEvent e)
    {
        return new TriggerKey(e.GetType().Name, e.GetType().Namespace);
    }
}
