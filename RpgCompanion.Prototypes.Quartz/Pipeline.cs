namespace RpgCompanion.QuartzPrototype;

using Core.Engine;

internal class Pipeline(IJobExecutionContext context, ISchedulerFactory schedulerFactory) : IPipeline
{
    public IPipeline<TEvent> Raise<TEvent>(TEvent e) where TEvent : Core.Events.IEvent
    {
        Canva.Test(schedulerFactory, e);
    }
}
