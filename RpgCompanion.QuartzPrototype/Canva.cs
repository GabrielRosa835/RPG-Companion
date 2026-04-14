namespace RpgCompanion.QuartzPrototype;

using Core.Events;

internal class Canva
{
    public static async Task Test(ISchedulerFactory schedulerFactory, IEvent e)
    {
        var scheduler = await schedulerFactory.GetScheduler();

        var triggerKey = e.ToTriggerKey();
        var jobKey = e.ToJobKey();

        ITrigger? trigger2 = await scheduler.GetTrigger(triggerKey);
        IJobDetail? job = await scheduler.GetJobDetail(jobKey);

        trigger2!.JobDataMap["event"] = e;

        await scheduler.ScheduleJob(job!, trigger2!);
    }
}
