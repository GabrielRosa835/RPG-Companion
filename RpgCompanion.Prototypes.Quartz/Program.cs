namespace RpgCompanion.QuartzPrototype;

using Core;
using Core.Engine;
using Core.Events;
using Microsoft.Extensions.Hosting;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddQuartz(quartz =>
        {
            quartz.AddJob<EffectJob<EmptyEvent, EmptyEffect>>(job =>
            {
                job.WithIdentity(nameof(EmptyEffect));
                job.PersistJobDataAfterExecution();
                job.DisallowConcurrentExecution();
            });
            quartz.AddJobListener<RuleListener<EmptyEvent, EmptyRule>>((sp, listener) =>
            {

            });
        });


        Console.WriteLine("Hello, World!");
    }
}
