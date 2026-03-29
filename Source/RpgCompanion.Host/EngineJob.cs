namespace RpgCompanion.Application;

using Engines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class EngineJob(IServiceProvider hostServices) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var process = hostServices.GetRequiredService<EventExecutionProcess>();
        return process.Run(stoppingToken);
    }
}
