namespace RpgCompanion.QuartzPrototype;

using Core.Events;

internal class EffectJobListener(ISchedulerFactory scheduler) : IJobListener
{
    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var e = (IEvent?) context.Result;
        if (e is not null)
        {
            await Canva.Test(scheduler, e);
        }
    }

    public string Name { get; }
}
